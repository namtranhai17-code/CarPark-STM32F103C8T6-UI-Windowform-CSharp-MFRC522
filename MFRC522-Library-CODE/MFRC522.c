#include "stm32f1xx_hal.h"
#include "MFRC522.h"
#include "string.h"

SPI_HandleTypeDef *_hspi;

/* ---- Low-level SPI helpers ---- */

static void CS_Low(void)
{
	HAL_GPIO_WritePin(MFRC522_CS_PORT, MFRC522_CS_PIN, GPIO_PIN_RESET);
}

static void CS_High(void)
{
	HAL_GPIO_WritePin(MFRC522_CS_PORT, MFRC522_CS_PIN, GPIO_PIN_SET);
}

static void MFRC522_WriteReg(uint8_t reg, uint8_t val)
{
	uint8_t addr = (reg << 1) & 0x7E;
	
	CS_Low();
	HAL_SPI_Transmit(_hspi, &addr, 1, 10);
	HAL_SPI_Transmit(_hspi, &val,  1, 10);
	CS_High();
}

static uint8_t MFRC522_ReadReg(uint8_t reg)
{
	uint8_t addr = ((reg << 1) & 0x7E) | 0x80;
	uint8_t val = 0, dummy = 0;
	CS_Low();
	HAL_SPI_Transmit(_hspi, &addr, 1, 10);
	HAL_SPI_TransmitReceive(_hspi, &dummy, &val, 1, 10);
	CS_High();
	return val;
}

static void MFRC522_SetBitMask(uint8_t reg, uint8_t mask){
	MFRC522_WriteReg(reg, MFRC522_ReadReg(reg) | mask);
}

static void MFRC522_ClearBitMask(uint8_t reg, uint8_t mask) {
	MFRC522_WriteReg(reg, MFRC522_ReadReg(reg) & ~mask);
}

/* ---- CRC ---- */

static void MFRC522_CalcCRC(uint8_t *data, uint8_t len, uint8_t *result) {
		MFRC522_ClearBitMask(0x05, 0x04);  // Clear CRCIrq
		// DivIrqReg = 0x05
		MFRC522_WriteReg(0x05, 0x04);
	
		MFRC522_SetBitMask(FIFOLevelReg, 0x80); // Flush FIFO
		for (uint8_t i = 0; i < len; i++)
				MFRC522_WriteReg(FIFODataReg, data[i]);
		MFRC522_WriteReg(CommandReg, PCD_CalcCRC);
	
		uint8_t i = 255;
		while (!(MFRC522_ReadReg(0x05) & 0x04) && --i);
		result[0] = MFRC522_ReadReg(CRCResultRegL);
		result[1] = MFRC522_ReadReg(CRCResultRegH);
}

/* ---- Core Transceive ---- */

static uint8_t MFRC522_ToCard(uint8_t cmd, uint8_t *sendData, uint8_t sendLen,
                               uint8_t *backData, uint16_t *backLen)
{
	uint8_t status = MI_ERR;
	uint8_t irqEn  = (cmd == PCD_Transceive) ? 0x77 : 0x12;
	uint8_t waitIRq= (cmd == PCD_Transceive) ? 0x30 : 0x10;
	
	MFRC522_WriteReg(ComIEnReg, irqEn | 0x80);
	MFRC522_ClearBitMask(ComIrqReg, 0x80);
	MFRC522_SetBitMask(FIFOLevelReg, 0x80);
	MFRC522_WriteReg(CommandReg, PCD_Idle);
	
	for (uint8_t i = 0; i < sendLen; i++)
		MFRC522_WriteReg(FIFODataReg, sendData[i]);
	
	MFRC522_WriteReg(CommandReg, cmd);
	if (cmd == PCD_Transceive)
		MFRC522_SetBitMask(BitFramingReg, 0x80);
	
	uint16_t i = 2000;
	uint8_t n;
	
	do { n = MFRC522_ReadReg(ComIrqReg); }
	while (--i && !(n & 0x01) && !(n & waitIRq));
	
	MFRC522_ClearBitMask(BitFramingReg, 0x80);
	
	if (i && !(MFRC522_ReadReg(ErrorReg) & 0x1B)) {
		status = MI_OK;
		if (n & irqEn & 0x01) status = MI_NOTAGERR;
		if (cmd == PCD_Transceive) {
			n = MFRC522_ReadReg(FIFOLevelReg);
			uint8_t lastBits = MFRC522_ReadReg(ControlReg) & 0x07;
			*backLen = lastBits ? (n - 1) * 8 + lastBits : n * 8;
			if (!n) n = 1;
			if (n > 16) n = 16;
			for (uint8_t i = 0; i < n; i++)
					backData[i] = MFRC522_ReadReg(FIFODataReg);
		}
	}
	return status;
}

/* ---- Public API ---- */

void MFRC522_Init(SPI_HandleTypeDef *hspi) {
	_hspi = hspi;
	HAL_GPIO_WritePin(MFRC522_RST_PORT, MFRC522_RST_PIN, GPIO_PIN_SET);
	CS_High();
	HAL_Delay(50);

	// Soft reset
	MFRC522_WriteReg(CommandReg, PCD_SoftReset);
	HAL_Delay(50);

	// Timer: auto-start, ~25ms timeout
	MFRC522_WriteReg(TModeReg,     0x8D);
	MFRC522_WriteReg(TPrescalerReg,0x3E);
	MFRC522_WriteReg(TReloadRegL,  0x1E);
	MFRC522_WriteReg(TReloadRegH,  0x00);
	MFRC522_WriteReg(TxASKReg,     0x40);
	MFRC522_WriteReg(ModeReg,      0x3D);

	// Enable antenna
	MFRC522_SetBitMask(TxControlReg, 0x03);
}

uint8_t MFRC522_Request(uint8_t reqMode, uint8_t *tagType) {
	uint16_t backBits;
	MFRC522_WriteReg(BitFramingReg, 0x07);
	tagType[0] = reqMode;
	uint8_t status = MFRC522_ToCard(PCD_Transceive, tagType, 1, tagType, &backBits);
	if (status != MI_OK || backBits != 0x10) status = MI_ERR;
	return status;
}

uint8_t MFRC522_Anticoll(uint8_t *serNum) {
	uint16_t backBits;
	uint8_t serNumCheck = 0;
	uint8_t buf[2] = {PICC_ANTICOLL, 0x20};
	MFRC522_WriteReg(BitFramingReg, 0x00);
	uint8_t status = MFRC522_ToCard(PCD_Transceive, buf, 2, serNum, &backBits);
	if (status == MI_OK)
			for (uint8_t i = 0; i < 4; i++) serNumCheck ^= serNum[i];
	if (serNumCheck != serNum[4]) status = MI_ERR;
	return status;
}

uint8_t MFRC522_SelectTag(uint8_t *serNum) {
    uint8_t buf[9] = {PICC_SELECTTAG, 0x70};
    uint8_t recvData[3];
    uint16_t recvBits;
    memcpy(&buf[2], serNum, 5);
    MFRC522_CalcCRC(buf, 7, &buf[7]);
    uint8_t status = MFRC522_ToCard(PCD_Transceive, buf, 9, recvData, &recvBits);
    return (status == MI_OK && recvBits == 0x18) ? recvData[0] : 0;
}

uint8_t MFRC522_Auth(uint8_t authMode, uint8_t blockAddr,
                     uint8_t *sectorKey, uint8_t *serNum) {
	uint8_t buf[12];
	uint16_t recvBits;
	uint8_t recvData[10];
	buf[0] = authMode;
	buf[1] = blockAddr;
	memcpy(&buf[2], sectorKey, 6);
	memcpy(&buf[8], serNum, 4);
	uint8_t status = MFRC522_ToCard(0x0E, buf, 12, recvData, &recvBits);
	// PCD_Authent = 0x0E
	if (!(MFRC522_ReadReg(0x08) & 0x08)) status = MI_ERR; // Status2Reg
	return status;
}
	 
uint8_t MFRC522_Read(uint8_t blockAddr, uint8_t *recvData) {
	uint8_t buf[4] = {PICC_READ, blockAddr};
	uint16_t recvBits;
	MFRC522_CalcCRC(buf, 2, &buf[2]);
	uint8_t status = MFRC522_ToCard(PCD_Transceive, buf, 4, recvData, &recvBits);
	return (status != MI_OK || recvBits != 0x90) ? MI_ERR : MI_OK;
}

uint8_t MFRC522_Write(uint8_t blockAddr, uint8_t *writeData) {
	uint8_t buf[18], recvData[4];
	uint16_t recvBits;
	buf[0] = PICC_WRITE; buf[1] = blockAddr;
	MFRC522_CalcCRC(buf, 2, &buf[2]);
	uint8_t status = MFRC522_ToCard(PCD_Transceive, buf, 4, recvData, &recvBits);
	if (status != MI_OK || recvBits != 4 || (recvData[0] & 0x0F) != 0x0A)
			return MI_ERR;
	memcpy(buf, writeData, 16);
	MFRC522_CalcCRC(buf, 16, &buf[16]);
	status = MFRC522_ToCard(PCD_Transceive, buf, 18, recvData, &recvBits);
	return (status != MI_OK || recvBits != 4 || (recvData[0] & 0x0F) != 0x0A) ? MI_ERR : MI_OK;
}

void MFRC522_StopCrypto(void) 
{ MFRC522_ClearBitMask(0x08, 0x08); }

void MFRC522_Halt(void) {
	uint8_t buf[4] = {PICC_HALT, 0};
	uint16_t bits;
	MFRC522_CalcCRC(buf, 2, &buf[2]);
	MFRC522_ToCard(PCD_Transceive, buf, 4, buf, &bits);
}

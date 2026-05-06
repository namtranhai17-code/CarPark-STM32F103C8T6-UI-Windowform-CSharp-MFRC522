#ifndef MFRC522_H
#define MFRC522_H

#ifdef __cplusplus
extern "C" {
#endif

#include "stm32f1xx_hal_spi.h"

// --- Pin Definitions (adjust to your setup) ---
#define MFRC522_CS_PORT		GPIOA
#define MFRC522_CS_PIN		GPIO_PIN_4
#define MFRC522_RST_PORT	GPIOB
#define MFRC522_RST_PIN		GPIO_PIN_0

// --- MFRC522 Register Map ---
#define CommandReg					0x01
#define ComIEnReg						0x02
#define ComIrqReg						0x04
#define ErrorReg						0x06
#define FIFODataReg					0x09
#define FIFOLevelReg				0x0A
#define ControlReg					0x0C
#define BitFramingReg				0x0D
#define ModeReg							0x11
#define TxControlReg				0x14
#define TxASKReg						0x15
#define CRCResultRegH				0x21
#define CRCResultRegL				0x22
#define TModeReg						0x2A
#define TPrescalerReg				0x2B
#define TReloadRegH					0x2C
#define TReloadRegL					0x2D

// --- MFRC522 Commands ---
#define PCD_Idle						0x00
#define PCD_CalcCRC					0x03
#define PCD_Transceive			0x0C
#define PCD_SoftReset				0x0F

// --- PICC Commands ---
#define PICC_REQIDL					0x26
#define PICC_ANTICOLL				0x93
#define PICC_SELECTTAG			0x93
#define PICC_AUTHENT1A			0x60
#define PICC_AUTHENT1B			0x61
#define PICC_READ						0x30
#define PICC_WRITE					0xA0
#define PICC_HALT						0x50

// --- Status Codes ---
#define MI_OK								0
#define MI_NOTAGERR					1
#define MI_ERR							2

// --- Default Key ---
#define MIFARE_KEY_A				{0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF}


// --- Function Prototypes ---
void			MFRC522_Init(SPI_HandleTypeDef *hspi);
uint8_t		MFRC522_Request(uint8_t reqMode, uint8_t *tagType);
uint8_t		MFRC522_Anticoll(uint8_t *serNum);
uint8_t		MFRC522_SelectTag(uint8_t *serNum);
uint8_t		MFRC522_Auth(uint8_t authMode, uint8_t blockAddr, uint8_t *sectorKey, uint8_t *serNum);
uint8_t		MFRC522_Read(uint8_t blockAddr, uint8_t *recvData);
uint8_t		MFRC522_Write(uint8_t blockAddr, uint8_t *writeData);
void			MFRC522_StopCrypto(void);
void			MFRC522_Halt(void);


#ifdef __cplusplus
}
#endif

#endif


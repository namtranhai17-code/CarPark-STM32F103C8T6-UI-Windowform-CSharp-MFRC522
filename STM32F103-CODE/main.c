#include "main.h"
#include "stdio.h"
#include "string.h"
#include "MFRC522.h"

I2C_HandleTypeDef hi2c1;
SPI_HandleTypeDef hspi1;
UART_HandleTypeDef huart1;

void SystemClock_Config(void);
static void MX_GPIO_Init(void);
static void MX_SPI1_Init(void);
static void MX_USART1_UART_Init(void);
static void MX_I2C1_Init(void);

/*------------------------------------------------------------------*/
void UART_Printf(const char *msg);
/*------------------------------------------------------------------*/
uint8_t MFRC522_Begin(void);
uint8_t MFRC522_WriteBlock(uint8_t block, uint8_t *data);
uint8_t MFRC522_ReadBlock(uint8_t block, uint8_t *outData);
void MFRC522_End(void);
/*------------------------------------------------------------------*/
uint8_t data_r[1];
uint8_t data_t[100];

uint8_t temp = 0;
uint8_t uiStop;
uint8_t cCard;
uint8_t mPark;
uint8_t inoutCheck = 0xff;

uint8_t cardRead = 0; uint8_t readSuccess = 0;

uint8_t servoR;
uint8_t servoL;

uint8_t cardPacket[9];   // C | 0xf0 | dd | MM | yy | dd | MM | yy | XOR
uint8_t packetIdx = 0;
uint8_t receivingPacket = 0;


/*------------------------------------------------------------------*/
uint8_t tagType[2];
uint8_t uid[5];			// Save UID Card (4 data bytes + 1 checksum byte)
uint8_t rData[18];	// the Array contains 16 readback data bytes + 2 CRC bytes
uint8_t wData[16];
char    msg[64];

static uint8_t g_UID[5];
static uint8_t g_key[6] = {0xFF,0xFF,0xFF,0xFF,0xFF,0xFF};
/*------------------------------------------------------------------*/
uint8_t createDate[16];
uint8_t expireDate[16];
/*------------------------------------------------------------------*/
int main(void)
{
	HAL_Init();
	SystemClock_Config();

	MX_GPIO_Init();
	MX_SPI1_Init();
	MX_USART1_UART_Init();
	MX_I2C1_Init();
	
	/* -- 1. Init MFRC522 -- */
	MFRC522_Init(&hspi1);
	
	for(int i = 0; i < 10; i++)
	{
		HAL_GPIO_WritePin(GPIOC, GPIO_PIN_13, GPIO_PIN_RESET);
		HAL_Delay(150);
		HAL_GPIO_WritePin(GPIOC, GPIO_PIN_13, GPIO_PIN_SET);
		HAL_Delay(150);
	}
	
	/* -- Interrupt UART -- */
	HAL_UART_Receive_IT(&huart1, data_r, 1);
	/*------------------------------------------------------*/
	UART_Printf("=== MFRC522 Ready ===\n");
	UART_Printf("=== PICC Card needs to be read/write ===\n");

  while (1)
  {
		/* User Interface Stop */
		if(uiStop == 0x50) /* Close UI */
		{
			cCard = 0;
			mPark = 0;
			servoL = 0;
			servoR = 0;
			
			HAL_GPIO_WritePin(GPIOC, GPIO_PIN_13, GPIO_PIN_SET);
		}
		else if(uiStop == 0x51) /* SerialPort connection on */
		{
			/* Feature */
			if(cCard == 0xf0) /* Create Month Card */
			{				
				if (MFRC522_Begin()) /* Scan 1 time */
				{
					snprintf((char*)wData, 16, "09052026|0|0");
					MFRC522_WriteBlock(4, wData);	HAL_Delay(10);

					snprintf((char*)wData, 16, "14|25|28|05|26");
					MFRC522_WriteBlock(5, wData);	HAL_Delay(10);

					snprintf((char*)wData, 16, "20|26|28|05|26");
					MFRC522_WriteBlock(6, wData);	HAL_Delay(10);

					MFRC522_WriteBlock(8, createDate);	HAL_Delay(10);

					MFRC522_WriteBlock(9, expireDate);	HAL_Delay(10);

					MFRC522_End(); /* Halt 1 time */
				}
				HAL_Delay(500);
			}
			else if(cCard == 0xf1) /* Create Week Card */
			{
				if (MFRC522_Begin()) /* Scan 1 time */
				{
					snprintf((char*)wData, 16, "09052026|1|0");
					MFRC522_WriteBlock(4, wData);	HAL_Delay(10);

					snprintf((char*)wData, 16, "11|11|28|05|26");
					MFRC522_WriteBlock(5, wData);	HAL_Delay(10);

					snprintf((char*)wData, 16, "22|22|28|05|26");
					MFRC522_WriteBlock(6, wData);	HAL_Delay(10);

					MFRC522_WriteBlock(8, createDate);	HAL_Delay(10);

					MFRC522_WriteBlock(9, expireDate);	HAL_Delay(10);

					MFRC522_End(); /* Halt 1 time */
				}
				HAL_Delay(500);
			}
			else if(cCard == 0xf2) /* Create Day Card */ /* Check Read function */
			{
				if (MFRC522_Begin()) /* Scan 1 time */
				{
					snprintf((char*)wData, 16, "09052026|2|0");
					MFRC522_WriteBlock(4, wData);	HAL_Delay(10);

					snprintf((char*)wData, 16, "22|33|28|05|26");
					MFRC522_WriteBlock(5, wData);	HAL_Delay(10);

					snprintf((char*)wData, 16, "11|44|29|05|26");
					MFRC522_WriteBlock(6, wData);	HAL_Delay(10);

					MFRC522_WriteBlock(8, createDate);	HAL_Delay(10);

					MFRC522_WriteBlock(9, expireDate);	HAL_Delay(10);

					MFRC522_End(); /* Halt 1 time */
				}
				HAL_Delay(500);
			}
			
			if(mPark == 0xff) /* Monitor Car Park */
			{
				if(!cardRead)
				{
					// ===== Read block and check data ====
					if (MFRC522_Begin())
					{
						readSuccess = 0;

						readSuccess += MFRC522_ReadBlock(4, rData); HAL_Delay(10); // key, type, inout
						readSuccess += MFRC522_ReadBlock(5, rData); HAL_Delay(10); // time in
						readSuccess += MFRC522_ReadBlock(6, rData); HAL_Delay(10); // time out
						readSuccess += MFRC522_ReadBlock(8, rData); HAL_Delay(10); // create date
						readSuccess += MFRC522_ReadBlock(9, rData); HAL_Delay(10); // expire date

						MFRC522_End();
						
						if(readSuccess == 5)
						{
							UART_Printf("R|0\n");  // OK
							cardRead = 1;          // Stop reading
							mPark    = 0x00;
						}
						else
						{
							UART_Printf("R|1\n"); // Error
						}
						HAL_Delay(100);
						
					}
				}
			}
			
			/* Control Servo */
			if(servoR == 0x21)
			{
				sprintf((char*)data_t, "A|%02X\n", servoR);
				HAL_UART_Transmit(&huart1, data_t, strlen((char*)data_t), 100);
				HAL_Delay(1000);
			}
			else if(servoR == 0x20)
			{
				sprintf((char*)data_t, "A|%02X\n", servoR);
				HAL_UART_Transmit(&huart1, data_t, strlen((char*)data_t), 100);
				
				HAL_Delay(1000);
			}
			
			if(servoL == 0x11)
			{
				sprintf((char*)data_t, "B|%02X\n", servoL);
				HAL_UART_Transmit(&huart1, data_t, 5, 100);
				HAL_Delay(1000);
			}
			else if(servoL == 0x10)
			{
				sprintf((char*)data_t, "B|%02X\n", servoL);
				HAL_UART_Transmit(&huart1, data_t, 5, 100);
				HAL_Delay(1000);
			}
			
			HAL_GPIO_WritePin(GPIOC, GPIO_PIN_13, GPIO_PIN_RESET);
		}
		else if(uiStop == 0x52) /* SerialPort connection off */
		{
			
			HAL_GPIO_WritePin(GPIOC, GPIO_PIN_13, GPIO_PIN_RESET);
			HAL_Delay(150);
			HAL_GPIO_WritePin(GPIOC, GPIO_PIN_13, GPIO_PIN_SET);
			HAL_Delay(150);
		}
  }
}
/*------------------------------------------------------------------*/
void UART_Printf(const char *msg) {
	HAL_UART_Transmit(&huart1, (uint8_t*)msg, strlen(msg), 100);
}
/*------------------------------------------------------------------*/
uint8_t MFRC522_Begin(void)
{
    uint8_t tagType[2];
    if (MFRC522_Request(PICC_REQIDL, tagType) != MI_OK)
    {
        return 0; /* Card has not been recognized yet */
    }
    if (MFRC522_Anticoll(g_UID) != MI_OK)
    {
        return 0;
    }
    MFRC522_SelectTag(g_UID);

    snprintf((char*)data_t, sizeof(data_t),
             "UID: %02X %02X %02X %02X\r\n",
             g_UID[0], g_UID[1], g_UID[2], g_UID[3]);
    UART_Printf((char*)data_t);

    return 1; /* Success */
}

uint8_t MFRC522_WriteBlock(uint8_t block, uint8_t *data)
{
    char msg[64];

    if (MFRC522_Auth(PICC_AUTHENT1A, block, g_key, g_UID) != MI_OK)
    {
        snprintf(msg, sizeof(msg), ">> Auth block %02d: Fail\r\n", block);
        UART_Printf(msg);
        return 0;
    }
    if (MFRC522_Write(block, data) != MI_OK)
    {
        snprintf(msg, sizeof(msg), ">> Write block %02d: Fail\r\n", block);
        UART_Printf(msg);
        return 0;
    }
    snprintf(msg, sizeof(msg), ">> Write block %02d: OK\r\n", block);
    UART_Printf(msg);
    return 1;
}

uint8_t MFRC522_ReadBlock(uint8_t block, uint8_t *outData)
{
	char msg[64];
	uint8_t buf[18];

	if (MFRC522_Auth(PICC_AUTHENT1A, block, g_key, g_UID) != MI_OK)
	{
	  snprintf(msg, sizeof(msg), "Auth block %02d: Fail\r\n", block);
	  UART_Printf(msg);
	  return 0;
	}
	if (MFRC522_Read(block, buf) != MI_OK)
	{
	  snprintf(msg, sizeof(msg), "Read block %02d: Fail\r\n", block);
	  UART_Printf(msg);
	  return 0;
	}

	buf[15] = '\0';
	snprintf(msg, sizeof(msg), "B%02d:%s\n", block, (char*)buf);
	UART_Printf(msg);

	if (outData != NULL) memcpy(outData, buf, 16);
	return 1;
}

void MFRC522_End(void)
{
    MFRC522_StopCrypto();
    MFRC522_Halt();
    UART_Printf("\r\n");
}
/*------------------------------------------------------------------*/
/* Call back reception data UART function */
void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart)
{
	/* Handle data from Transmission frame */
	if(receivingPacket)
	{
		uint8_t buff = data_r[0];
		
		cardPacket[packetIdx++] = buff;
		
		if(packetIdx == 8)
		{
			uint8_t create_dd = cardPacket[2];
			uint8_t create_MM = cardPacket[3];
			uint8_t create_yy = cardPacket[4];
			uint8_t expire_dd = cardPacket[5];
			uint8_t expire_MM = cardPacket[6];
			uint8_t expire_yy = cardPacket[7];
			
			if		 (cardPacket[1] == 0xf0) cCard = 0xf0;		// Month Card
			else if(cardPacket[1] == 0xf1) cCard = 0xf1; 	// Week  Card
			else if(cardPacket[1] == 0xf2) cCard = 0xf2; 	// Day   Card
			// block 8: "C|30|04|26"
			snprintf((char*)createDate, sizeof(createDate),
                         "C|%02d|%02d|%02d", create_dd, create_MM, create_yy);
			// block 9: "E|30|05|26"
			snprintf((char*)expireDate, sizeof(expireDate),
                         "E|%02d|%02d|%02d", expire_dd, expire_MM, expire_yy);
			
			UART_Printf("Packet OK\r\n");
			receivingPacket = 0;
			packetIdx = 0;
			
			HAL_UART_Receive_IT(&huart1, data_r, 1);
			return; /* get out early */
		}
	}
	
	/* Handle Time Data from Transmission frame */
	/* [InOut|HH|mm|dd|MM|yy] = 6 bytes */
	
	/* Assign Data */
	/* Servo */
	if(temp == 'A')
	{
		servoR = data_r[0];
		cCard = 0x00;
		mPark = 0x00;
	}
	if(temp == 'B')
	{
		servoL = data_r[0];
		cCard = 0x00;
		mPark = 0x00;
	}
	/* Feature */
	if(temp == 'C')
	{
		if(data_r[0] == 0xf0 || data_r[0] == 0xf1 || data_r[0] == 0xf2)
		{
			receivingPacket = 1;
			packetIdx = 0;
			cardPacket[0] = (uint8_t)'C';
			cardPacket[1] = data_r[0];
			packetIdx = 2; /* Start receiving data from 2nd Frame */
		}
		mPark = 0x00;
		servoR = 0x00;
		servoL = 0x00;
	}
	if(temp == 'M')
	{
		mPark = data_r[0];
		cCard = 0x00;
		if(mPark == 0x20 || mPark == 0x21)
		{
			servoR = mPark;
		}
		if(mPark == 0x10 || mPark == 0x11)
		{
			servoL = mPark;
		}
		cardRead = 0;
	}
	if(temp == 'T')
	{
		inoutCheck = data_r[0];
	}
	/* User Interface Stop */
	if(temp == 'S')
	{
		uiStop = data_r[0];
	}
	
	/* Detect Data */
	if		 (data_r[0] == 'A') temp = data_r[0];	// Barie R
	else if(data_r[0] == 'B') temp = data_r[0];	// Barie L
	else if(data_r[0] == 'C') temp = data_r[0];	// Create Card
	else if(data_r[0] == 'M') temp = data_r[0];	// Monitor Car Park
	else if(data_r[0] == 'T') temp = data_r[0];	// In Out
	else if(data_r[0] == 'S') temp = data_r[0];
	else 							  temp = 0;

	/* Continue Interrupt Reception */
	HAL_UART_Receive_IT(&huart1, data_r, 1);
	return;
}
/*------------------------------------------------------------------*/
/**
  * @brief System Clock Configuration
  * @retval None
  */
void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};

  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
  RCC_OscInitStruct.HSEState = RCC_HSE_ON;
  RCC_OscInitStruct.HSEPredivValue = RCC_HSE_PREDIV_DIV1;
  RCC_OscInitStruct.HSIState = RCC_HSI_ON;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
  RCC_OscInitStruct.PLL.PLLMUL = RCC_PLL_MUL9;
  if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
  {
    Error_Handler();
  }

  /** Initializes the CPU, AHB and APB buses clocks
  */
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV2;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;

  if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_2) != HAL_OK)
  {
    Error_Handler();
  }
}

/**
  * @brief I2C1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_I2C1_Init(void)
{

  /* USER CODE BEGIN I2C1_Init 0 */

  /* USER CODE END I2C1_Init 0 */

  /* USER CODE BEGIN I2C1_Init 1 */

  /* USER CODE END I2C1_Init 1 */
  hi2c1.Instance = I2C1;
  hi2c1.Init.ClockSpeed = 100000;
  hi2c1.Init.DutyCycle = I2C_DUTYCYCLE_2;
  hi2c1.Init.OwnAddress1 = 0;
  hi2c1.Init.AddressingMode = I2C_ADDRESSINGMODE_7BIT;
  hi2c1.Init.DualAddressMode = I2C_DUALADDRESS_DISABLE;
  hi2c1.Init.OwnAddress2 = 0;
  hi2c1.Init.GeneralCallMode = I2C_GENERALCALL_DISABLE;
  hi2c1.Init.NoStretchMode = I2C_NOSTRETCH_DISABLE;
  if (HAL_I2C_Init(&hi2c1) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN I2C1_Init 2 */

  /* USER CODE END I2C1_Init 2 */

}

/**
  * @brief SPI1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_SPI1_Init(void)
{

  /* USER CODE BEGIN SPI1_Init 0 */

  /* USER CODE END SPI1_Init 0 */

  /* USER CODE BEGIN SPI1_Init 1 */

  /* USER CODE END SPI1_Init 1 */
  /* SPI1 parameter configuration*/
  hspi1.Instance = SPI1;
  hspi1.Init.Mode = SPI_MODE_MASTER;
  hspi1.Init.Direction = SPI_DIRECTION_2LINES;
  hspi1.Init.DataSize = SPI_DATASIZE_8BIT;
  hspi1.Init.CLKPolarity = SPI_POLARITY_LOW;
  hspi1.Init.CLKPhase = SPI_PHASE_1EDGE;
  hspi1.Init.NSS = SPI_NSS_SOFT;
  hspi1.Init.BaudRatePrescaler = SPI_BAUDRATEPRESCALER_16;
  hspi1.Init.FirstBit = SPI_FIRSTBIT_MSB;
  hspi1.Init.TIMode = SPI_TIMODE_DISABLE;
  hspi1.Init.CRCCalculation = SPI_CRCCALCULATION_DISABLE;
  hspi1.Init.CRCPolynomial = 10;
  if (HAL_SPI_Init(&hspi1) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN SPI1_Init 2 */

  /* USER CODE END SPI1_Init 2 */

}

/**
  * @brief USART1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_USART1_UART_Init(void)
{

  /* USER CODE BEGIN USART1_Init 0 */

  /* USER CODE END USART1_Init 0 */

  /* USER CODE BEGIN USART1_Init 1 */

  /* USER CODE END USART1_Init 1 */
  huart1.Instance = USART1;
  huart1.Init.BaudRate = 115200;
  huart1.Init.WordLength = UART_WORDLENGTH_8B;
  huart1.Init.StopBits = UART_STOPBITS_1;
  huart1.Init.Parity = UART_PARITY_NONE;
  huart1.Init.Mode = UART_MODE_TX_RX;
  huart1.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  huart1.Init.OverSampling = UART_OVERSAMPLING_16;
  if (HAL_UART_Init(&huart1) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN USART1_Init 2 */

  /* USER CODE END USART1_Init 2 */

}

/**
  * @brief GPIO Initialization Function
  * @param None
  * @retval None
  */
static void MX_GPIO_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStruct = {0};
  /* USER CODE BEGIN MX_GPIO_Init_1 */

  /* USER CODE END MX_GPIO_Init_1 */

  /* GPIO Ports Clock Enable */
  __HAL_RCC_GPIOC_CLK_ENABLE();
  __HAL_RCC_GPIOD_CLK_ENABLE();
  __HAL_RCC_GPIOA_CLK_ENABLE();
  __HAL_RCC_GPIOB_CLK_ENABLE();

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOC, GPIO_PIN_13, GPIO_PIN_RESET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOA, GPIO_PIN_4, GPIO_PIN_SET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOB, GPIO_PIN_0, GPIO_PIN_SET);

  /*Configure GPIO pin : PC13 */
  GPIO_InitStruct.Pin = GPIO_PIN_13;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOC, &GPIO_InitStruct);

  /*Configure GPIO pin : PA4 */
  GPIO_InitStruct.Pin = GPIO_PIN_4;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
  HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);

  /*Configure GPIO pin : PB0 */
  GPIO_InitStruct.Pin = GPIO_PIN_0;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
  HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

  /* USER CODE BEGIN MX_GPIO_Init_2 */

  /* USER CODE END MX_GPIO_Init_2 */
}

/* USER CODE BEGIN 4 */

/* USER CODE END 4 */

/**
  * @brief  This function is executed in case of error occurrence.
  * @retval None
  */
void Error_Handler(void)
{
  /* USER CODE BEGIN Error_Handler_Debug */
  /* User can add his own implementation to report the HAL error return state */
  __disable_irq();
  while (1)
  {
  }
  /* USER CODE END Error_Handler_Debug */
}
#ifdef USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */

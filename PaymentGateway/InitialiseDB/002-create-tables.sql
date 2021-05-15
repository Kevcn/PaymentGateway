CREATE SCHEMA `payment_gateway`;
USE payment_gateway;

CREATE TABLE `payment_details` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `CardNumber` varchar(16) NOT NULL,
  `ExpiryMonth` int NOT NULL,
  `ExpiryDate` int NOT NULL,
  `CardHolderName` varchar(255) NOT NULL,
  `Amount` decimal(18,6) NOT NULL,
  `Currency` varchar(3) NOT NULL,
  `CVV` varchar(3) NOT NULL,
  PRIMARY KEY (`ID`)
);

CREATE TABLE `transaction_details` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `TransactionID` bigint NOT NULL,
  `Success` bool NOT NULL,
  `PaymentDetailsID` int NOT NULL,  
  `CreatedDate` datetime NOT NULL,
   PRIMARY KEY (`ID`)
)
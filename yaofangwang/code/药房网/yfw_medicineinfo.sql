/*
Navicat MySQL Data Transfer

Source Server         : 药房网MySQL
Source Server Version : 50540
Source Host           : 172.19.1.56:3306
Source Database       : pos

Target Server Type    : MYSQL
Target Server Version : 50540
File Encoding         : 65001

Date: 2020-04-24 15:23:17
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for yfw_medicineinfo
-- ----------------------------
DROP TABLE IF EXISTS `yfw_medicineinfo`;
CREATE TABLE `yfw_medicineinfo` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `PrimaryKey` varchar(50) DEFAULT NULL,
  `Namecn` varchar(50) DEFAULT NULL,
  `Aliascn` varchar(50) DEFAULT NULL,
  `Standard` varchar(50) DEFAULT NULL,
  `TrocheType` varchar(50) DEFAULT NULL,
  `AuthorizedCode` varchar(50) DEFAULT NULL,
  `Discount` varchar(10) DEFAULT NULL,
  `ProductBarcode` varchar(50) DEFAULT NULL,
  `Category` varchar(10) DEFAULT NULL,
  `Milltitle` varchar(100) DEFAULT NULL,
  `Weight` varchar(10) DEFAULT NULL,
  `ReceivePrice` varchar(15) DEFAULT NULL,
  `Price` varchar(15) DEFAULT NULL,
  `MaxShelfStock` varchar(15) DEFAULT NULL,
  `AvailableStock` varchar(15) DEFAULT NULL,
  `Stock` varchar(15) DEFAULT NULL,
  `ProduceDate` varchar(50) DEFAULT NULL,
  `PeriodTo` varchar(50) DEFAULT NULL,
  `Unit` varchar(50) DEFAULT NULL,
  `ProductBatchNo` varchar(50) DEFAULT NULL,
  `ConversionRatio` varchar(10) DEFAULT NULL,
  `ProductNumber` varchar(50) DEFAULT NULL,
  `MaxBuyQuantity` varchar(15) DEFAULT NULL,
  `SendDay` varchar(10) DEFAULT NULL,
  `StatusId` varchar(10) DEFAULT NULL,
  `StockUpdateCode` int(11) DEFAULT NULL,
  `StockUpdateMsg` varchar(100) DEFAULT NULL,
  `PriceUpdateCode` int(11) DEFAULT NULL,
  `PriceUpdateMsg` varchar(100) DEFAULT NULL,
  `BasicUpdateCode` int(11) DEFAULT NULL,
  `BasicUpdateMsg` varchar(100) DEFAULT NULL,
  `BasicInsertCode` int(11) DEFAULT NULL,
  `BasicInsertMsg` varchar(100) DEFAULT NULL,
  `SapStockGetCode` int(11) DEFAULT NULL,
  `SapStockGetMsg` varchar(100) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  `ModifyTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `index_01` (`PrimaryKey`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1401 DEFAULT CHARSET=gbk;

/*
Navicat MySQL Data Transfer

Source Server         : 药房网MySQL
Source Server Version : 50540
Source Host           : 172.19.1.56:3306
Source Database       : pos

Target Server Type    : MYSQL
Target Server Version : 50540
File Encoding         : 65001

Date: 2020-04-24 15:23:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for yfw_billproinfo
-- ----------------------------
DROP TABLE IF EXISTS `yfw_billproinfo`;
CREATE TABLE `yfw_billproinfo` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `OrderNo` varchar(50) DEFAULT NULL,
  `PrimaryKey` varchar(50) DEFAULT NULL,
  `ProductNumber` varchar(50) DEFAULT NULL,
  `Aliascn` varchar(50) DEFAULT NULL,
  `Standard` varchar(50) DEFAULT NULL,
  `TrocheType` varchar(50) DEFAULT NULL,
  `MillTitle` varchar(50) DEFAULT NULL,
  `ProduceNo` varchar(50) DEFAULT NULL,
  `UnitPrice` varchar(15) DEFAULT NULL,
  `Quantity` varchar(10) DEFAULT NULL,
  `Total` varchar(20) DEFAULT NULL,
  `ReturnQuantity` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=329 DEFAULT CHARSET=gbk;

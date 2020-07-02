/*
Navicat MySQL Data Transfer

Source Server         : 药房网
Source Server Version : 50540
Source Host           : 172.19.1.56:3306
Source Database       : pos

Target Server Type    : MYSQL
Target Server Version : 50540
File Encoding         : 65001

Date: 2020-06-10 10:14:30
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for yfw_billinfo
-- ----------------------------
DROP TABLE IF EXISTS `yfw_billinfo`;
CREATE TABLE `yfw_billinfo` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `OrderNo` varchar(50) DEFAULT NULL,
  `StatusId` varchar(5) DEFAULT NULL,
  `StatusName` varchar(20) DEFAULT NULL,
  `OrderTotal` varchar(30) DEFAULT NULL,
  `OrderType` varchar(20) DEFAULT NULL,
  `NeedAuditRx` varchar(30) DEFAULT NULL,
  `PayType` varchar(20) DEFAULT NULL,
  `BuyerName` varchar(50) DEFAULT NULL,
  `BuyerPhone` varchar(50) DEFAULT NULL,
  `BuyerMobile` varchar(50) DEFAULT NULL,
  `ReceiverName` varchar(50) DEFAULT NULL,
  `ReceiverPhone` varchar(50) DEFAULT NULL,
  `ReceiverMobile` varchar(50) DEFAULT NULL,
  `ReceiverEmail` varchar(50) DEFAULT NULL,
  `ReceiverAddress` varchar(200) DEFAULT NULL,
  `ReceiverRegion` varchar(50) DEFAULT NULL,
  `IsPrescription` varchar(10) DEFAULT NULL,
  `IsConfirm` int(11) DEFAULT '0',
  `OrderTime` varchar(30) NOT NULL,
  `CreateTime` datetime DEFAULT NULL,
  `ModifyTime` datetime DEFAULT NULL,
  `ConfirmTime` datetime DEFAULT NULL,
  `IsDelete` int(11) DEFAULT '0',
  `Platform` varchar(20) DEFAULT NULL COMMENT 'yaofangwang',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `index_01` (`OrderNo`) USING BTREE COMMENT '订单号唯一'
) ENGINE=InnoDB AUTO_INCREMENT=439 DEFAULT CHARSET=gbk;

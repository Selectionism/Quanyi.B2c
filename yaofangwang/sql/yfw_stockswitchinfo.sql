/*
Navicat MySQL Data Transfer

Source Server         : 药房网
Source Server Version : 50540
Source Host           : 172.19.1.56:3306
Source Database       : pos

Target Server Type    : MYSQL
Target Server Version : 50540
File Encoding         : 65001

Date: 2020-06-22 13:11:58
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for yfw_stockswitchinfo
-- ----------------------------
DROP TABLE IF EXISTS `yfw_stockswitchinfo`;
CREATE TABLE `yfw_stockswitchinfo` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `Checked` int(11) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=gbk;

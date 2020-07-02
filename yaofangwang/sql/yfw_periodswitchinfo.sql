/*
Navicat MySQL Data Transfer

Source Server         : 药房网
Source Server Version : 50540
Source Host           : 172.19.1.56:3306
Source Database       : pos

Target Server Type    : MYSQL
Target Server Version : 50540
File Encoding         : 65001

Date: 2020-06-22 13:11:50
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for yfw_periodswitchinfo
-- ----------------------------
DROP TABLE IF EXISTS `yfw_periodswitchinfo`;
CREATE TABLE `yfw_periodswitchinfo` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `Checked` int(11) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=gbk;

-- MySQL dump 10.13  Distrib 5.5.20, for Win64 (x86)
--
-- Host: localhost    Database: pundit
-- ------------------------------------------------------
-- Server version	5.5.20

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `option`
--

DROP TABLE IF EXISTS `option`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `option` (
  `OptionId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Value` text,
  PRIMARY KEY (`OptionId`),
  UNIQUE KEY `Option_Key` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `packagedependency`
--

DROP TABLE IF EXISTS `packagedependency`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `packagedependency` (
  `PackageDependencyId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `PackageManifestId` int(10) unsigned NOT NULL,
  `PackageId` varchar(45) NOT NULL,
  `VersionPattern` varchar(45) NOT NULL,
  `Platform` varchar(45) NOT NULL,
  `Scope` tinyint(3) unsigned NOT NULL,
  `CreatePlatformFolder` tinyint(1) NOT NULL,
  PRIMARY KEY (`PackageDependencyId`),
  KEY `FK_PackageDepenency_PackageManifest` (`PackageManifestId`),
  CONSTRAINT `FK_PackageDepenency_PackageManifest` FOREIGN KEY (`PackageManifestId`) REFERENCES `packagemanifest` (`PackageManifestId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `packagelog`
--

DROP TABLE IF EXISTS `packagelog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `packagelog` (
  `PackageLogId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `ModType` tinyint(3) unsigned NOT NULL,
  `ModTime` datetime NOT NULL,
  `PackageManifestId` int(10) unsigned NOT NULL,
  `PackageId` varchar(45) NOT NULL,
  `Platform` varchar(45) NOT NULL,
  `Version` varchar(45) NOT NULL,
  PRIMARY KEY (`PackageLogId`) USING BTREE,
  KEY `FK_manifesthistory_packagemanifest` (`PackageManifestId`) USING BTREE,
  KEY `PackageLog_Key` (`PackageId`,`Platform`,`Version`),
  CONSTRAINT `FK_manifesthistory_packagemanifest` FOREIGN KEY (`PackageManifestid`) REFERENCES `packagemanifest` (`PackageManifestId`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `packagemanifest`
--

DROP TABLE IF EXISTS `packagemanifest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `packagemanifest` (
  `PackageManifestId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `PackageId` varchar(255) NOT NULL,
  `Version` varchar(45) NOT NULL,
  `Platform` varchar(45) NOT NULL,
  `ProjectUrl` varchar(255) DEFAULT NULL,
  `Author` varchar(45) DEFAULT NULL,
  `Description` text,
  `ReleaseNotes` text,
  `License` text,
  `IsActive` tinyint(1) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `FileSize` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`PackageManifestId`),
  KEY `PackageManifest_Key` (`PackageId`,`Version`,`Platform`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-06-14 21:07:35

CREATE FUNCTION SPLIT_STR(
  x VARCHAR(255),
  delim VARCHAR(12),
  pos INT
)
RETURNS VARCHAR(255)
RETURN REPLACE(SUBSTRING(SUBSTRING_INDEX(x, delim, pos),
       LENGTH(SUBSTRING_INDEX(x, delim, pos -1)) + 1),
       delim, '');

ALTER TABLE `packagemanifest` ADD COLUMN `VMaj` INTEGER UNSIGNED NOT NULL AFTER `DownloadCount`,
 ADD COLUMN `VMin` INTEGER UNSIGNED NOT NULL AFTER `VMaj`,
 ADD COLUMN `VBld` INTEGER UNSIGNED NOT NULL AFTER `VMin`,
 ADD COLUMN `VRev` INTEGER UNSIGNED NOT NULL AFTER `VBld`;

update packagemanifest
set
VMaj=cast(split_str(Version, '.', 1) as signed),
VMin=cast(split_str(Version, '.', 2) as signed),
VBld=cast(split_str(Version, '.', 3) as signed),
VRev=cast(split_str(Version, '.', 4) as signed);

ALTER TABLE `pundit`.`packagemanifest` DROP COLUMN `Version`,
 DROP COLUMN `VersionShort`,
 MODIFY COLUMN `VMaj` INT(10) UNSIGNED DEFAULT NULL,
 MODIFY COLUMN `VMin` INT(10) UNSIGNED DEFAULT NULL,
 MODIFY COLUMN `VBld` INT(10) UNSIGNED DEFAULT NULL,
 MODIFY COLUMN `VRev` INT(10) UNSIGNED DEFAULT NULL,
 DROP INDEX `PackageManifest_Key`,
 ADD INDEX `PackageManifest_Key` USING BTREE(`PackageId`, `Platform`),
 DROP INDEX `PackageManifest_Revisions`,
 ADD INDEX `PackageManifest_Revisions` USING BTREE(`PackageId`, `Platform`);

update option set value='2' where name='version';
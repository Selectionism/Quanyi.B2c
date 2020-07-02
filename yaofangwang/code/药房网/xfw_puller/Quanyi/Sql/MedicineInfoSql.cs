using MySql.Data.MySqlClient;
using Quanyi.Entity.DBEntity.Medicine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Quanyi.Sql
{
    public class MedicineInfoSql
    {
		/// <summary>
		/// 插入商品资料表SQL语句
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
        public StringBuilder InsertMedicine(MedicineInfo entity)
        {
			var sqlInsertMedicineInfo = new StringBuilder();
			sqlInsertMedicineInfo.Append($@"INSERT INTO yfw_medicineinfo (
	PrimaryKey,Namecn,Aliascn,Standard,TrocheType,AuthorizedCode,Discount,
	ProductBarcode,Milltitle,Weight,Price,MaxShelfStock,
	AvailableStock,Stock,ProduceDate,PeriodTo,Unit,ProductBatchNo,CreateTime,ProductNumber,MaxBuyQuantity,SendDay,StatusId)
	VALUES('{entity.PrimaryKey}','{entity.Namecn}','{entity.Aliascn}','{entity.Standard}',
	'{entity.TrocheType}','{entity.AuthorizedCode}','{entity.Discount}','{entity.ProductBarcode}',
	'{entity.Milltitle}','{entity.Weight}','{entity.Price}',
	'{entity.MaxShelfStock}','{entity.AvailableStock}','{entity.Stock}','{entity.ProduceDate}','{entity.PeriodTo}',
	'{entity.Unit}','{entity.ProductBatchNo}','{entity.CreateTime}','{entity.ProductNumber}','{entity.MaxBuyQuantity}','{entity.SendDay}','{entity.StatusId}');");
			return sqlInsertMedicineInfo;
		}

		/// <summary>
		/// 更新商品资料表SQL语句
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public StringBuilder UpdateMedicine(MedicineInfo entity)
		{
			var sqlUpdateMedicineInfo = new StringBuilder();
			sqlUpdateMedicineInfo.Append($@"UPDATE yfw_medicineinfo
SET Namecn='{entity.Namecn}',
Aliascn='{entity.Aliascn}',
Standard='{entity.Standard}',
TrocheType='{entity.TrocheType}',
AuthorizedCode='{entity.AuthorizedCode}',
Discount='{entity.Discount}',
ProductBarcode='{entity.ProductBarcode}',
ProductNumber='{entity.ProductNumber}',
Milltitle='{entity.Milltitle}',
Weight='{entity.Weight}',
Price='{entity.Price}',
MaxShelfStock='{entity.MaxShelfStock}',
AvailableStock='{entity.AvailableStock}',
Stock='{entity.Stock}',
ProduceDate='{entity.ProduceDate}',
MaxBuyQuantity='{entity.MaxBuyQuantity}',
SendDay='{entity.SendDay}',
StatusId='{entity.StatusId}',
PeriodTo='{entity.PeriodTo}',
Unit='{entity.Unit}',
ProductBatchNo='{entity.ProductBatchNo}',
ModifyTime='{entity.ModifyTime}'
WHERE PrimaryKey='{entity.PrimaryKey}';");
			return sqlUpdateMedicineInfo;
		}

		public StringBuilder UpdateMedicineForStock(List<MedicineInfo> list)
		{
			var sql = new StringBuilder();
			foreach (var item in list)
			{
				sql.Append($@"UPDATE yfw_medicineinfo
SET AvailableStock='{item.AvailableStock}',
Stock='{item.Stock}',ModifyTime='{DateTime.Now}'
WHERE PrimaryKey='{item.PrimaryKey}';");
			}
			return sql;
		}

		/// <summary>
		/// 获取POS表中对应的可用商品集合(针对药房网，药房网资料表对应记录)
		/// </summary>
		/// <returns></returns>
		public StringBuilder GetPosAvailMedicineList()
		{
			var sql = new StringBuilder();
			sql.Append(@"SELECT
	s.st_pro_id PrimaryKey,
	s.st_on_hand_qty AvailableStock,
	b.batch_no ProductBatchNo,
	b.valid_date PeriodTo
FROM
	store s
LEFT JOIN (
	SELECT
		sb_pro_id,
		MAX(sb_batch_no) AS batch_no,
		MAX(sb_valid_date) AS valid_date
	FROM
		store_batch
	GROUP BY
		sb_pro_id
) b ON s.st_pro_id = b.sb_pro_id
WHERE
	s.st_pro_id IN (
		SELECT
			PrimaryKey
		FROM
			yfw_medicineinfo
	);");
			return sql;
		}

		public StringBuilder UpdateMedicineForSap(List<MedicineInfo> list)
		{
			var sql = new StringBuilder();
			foreach (var item in list)
			{
				sql.Append($@"UPDATE yfw_medicineinfo
SET PeriodTo='{item.PeriodTo}',
ProduceDate='{item.ProduceDate}',
ProductBatchNo='{item.ProductBatchNo}'
WHERE PrimaryKey='{item.PrimaryKey}';");
			}
			return sql;
		}

		public StringBuilder MedicineUpdate(int type, MedicineInfo entity)
		{
			var sql = new StringBuilder();
			switch(type)
			{
				case 0:
					sql.Append($@"UPDATE yfw_medicineinfo
SET StatusId='1'
WHERE PrimaryKey='{entity.PrimaryKey}';");
					break;
				case 1:
					sql.Append($@"UPDATE yfw_medicineinfo
SET StatusId='-999'
WHERE PrimaryKey='{entity.PrimaryKey}';");
					break;
				case 2:
					sql.Append($@"DELETE FROM yfw_medicineinfo
WHERE PrimaryKey='{entity.PrimaryKey}';");
					break;
			}
			return sql;
		}

		public StringBuilder SetSwitchInfo(int type, int cbVal, List<DbParameter> dbParameters)
        {
			StringBuilder sql = new StringBuilder();
			switch(type)
            {
				case 0://库存
					sql.Append(@"INSERT INTO yfw_stockswitchinfo (Checked, CreateTime)
VALUES(@Checked ,@CreateTime);");
					break;
				default://效期
					sql.Append(@"INSERT INTO yfw_periodswitchinfo (Checked, CreateTime)
VALUES(@Checked ,@CreateTime);");
					break;
            }
			dbParameters.Add(new MySqlParameter()
			{
				ParameterName = $"@Checked",
				DbType = DbType.String,
				Direction = ParameterDirection.Input,
				Value = cbVal
			});
			dbParameters.Add(new MySqlParameter()
			{
				ParameterName = $"@CreateTime",
				DbType = DbType.DateTime,
				Direction = ParameterDirection.Input,
				Value = DateTime.Now
			});
			return sql;
        }

		/// <summary>
		/// 获取商品资料表已有记录
		/// </summary>
		/// <param name="primaryKeyList"></param>
		/// <returns></returns>
		public StringBuilder GetMedicineList(string primaryKeyList="")
		{
			var sqlMedicine = new StringBuilder();
			if (primaryKeyList=="")
			{
				
				sqlMedicine.Append(@"SELECT * FROM yfw_medicineinfo;");
			}
			else
			{
				sqlMedicine.Append($@"SELECT * FROM yfw_medicineinfo
WHERE PrimaryKey IN ('{primaryKeyList}');");
			}
			return sqlMedicine;
		}

		public StringBuilder GetMedicineListForStockUpdate()
		{
			var sql = new StringBuilder();
			sql.Append(@"SELECT PrimaryKey,MaxShelfStock,AvailableStock FROM yfw_medicineinfo;");
			return sql;
		}

		public StringBuilder GetPosAvailStockInfo(string primaryKey)
		{
			var sql = new StringBuilder();
			sql.Append($@"SELECT
	s.st_pro_id PrimaryKey,
	CAST(CAST(s.st_on_hand_qty AS UNSIGNED) AS CHAR(15)) AvailableStock,
	b.batch_no ProductBatchNo,
	DATE_FORMAT(
		b.valid_date,
		'%Y-%m-%d %H:%i:%s'
	) PeriodTo
FROM
	store s
LEFT JOIN (
	SELECT
		sb_pro_id,
		MAX(sb_batch_no) AS batch_no,
		MAX(sb_valid_date) AS valid_date
	FROM
		store_batch
	GROUP BY
		sb_pro_id
) b ON s.st_pro_id = b.sb_pro_id
WHERE s.st_pro_id IN ('{primaryKey}');");
			return sql;
		}

		public StringBuilder GetMedicineListForPeriodUpdate()
		{
			var sql = new StringBuilder();
			sql.Append(@"SELECT PrimaryKey,PeriodTo,ProduceDate,ProductBatchNo FROM yfw_medicineinfo;");
			return sql;
		}

		public StringBuilder GetMedicineInfo(MedicineInfo entity)
		{
			var sql = new StringBuilder();
			sql.Append($@"SELECT * FROM yfw_medicineinfo
WHERE PrimaryKey='{entity.PrimaryKey}';");
			return sql;
		}

		public StringBuilder UpdateMedicinePrice(MedicineInfo entity)
		{
			var sql = new StringBuilder();
			sql.Append($@"UPDATE yfw_medicineinfo
SET Price='{entity.Price}'
WHERE PrimaryKey='{entity.PrimaryKey}';");
			return sql;
		}
	}
}

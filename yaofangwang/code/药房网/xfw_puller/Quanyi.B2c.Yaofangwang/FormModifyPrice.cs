using Quanyi.Entity.DBEntity.Medicine;
using Quanyi.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanyi.B2c.Yaofangwang
{
    public partial class FormModifyPrice : Form
    {
        private RegexHelper _regexHelper;
        private B2cMedicine _medicineService;
        public FormModifyPrice(string currPrimaryKey, string currentPrice)
        {
            InitializeComponent();
            //调用接口？还是直接窗体传入当前价格过来？
            lblPrimaryKey.Text = currPrimaryKey;
            lblCurrentPrice.Text = currentPrice;
            _regexHelper = new RegexHelper();
            _medicineService = new B2cMedicine();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            //正则表达式验证是否输入的是数字
            if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
            {
                MessageBox.Show("请输入价格！");
                return;
            }
            var f = _regexHelper.IsFloat(txtPrice.Text.Trim());
            var i= _regexHelper.IsInteger(txtPrice.Text.Trim());
            if (!f && !i)
            {
                MessageBox.Show("价格必须是数值类型！");
                return;
            }
            var entity = new MedicineInfo()
            {
                PrimaryKey = lblPrimaryKey.Text,
                Price = txtPrice.Text.Trim(),
            };
            string msg;
            var result = _medicineService.PriceUpdate(entity, out msg);
            if(result)
            {
                MessageBox.Show("价格更新成功");
            }
            else
            {
                MessageBox.Show(msg);
            }
        }
    }
}

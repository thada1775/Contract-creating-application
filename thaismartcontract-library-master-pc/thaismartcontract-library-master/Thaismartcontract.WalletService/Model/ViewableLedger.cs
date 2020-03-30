using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thaismartcontract.WalletService.Model
{
    public class ViewableLedger
    {
        [DisplayName("สถานะ")]
        public string Status { get; set; }
        [DisplayName("เวลา")]
        public DateTime Time { get; set; }
        [DisplayName("ผู้ส่ง")]
        public string Sender { get; set; }
        [DisplayName("ดำเนินการ")]
        public string Operation { get; set; }
        [DisplayName("ผู้รับ")]
        public string Receiver { get; set; }
        [DisplayName("จำนวน")]
        public decimal Amount { get; set; }
        [DisplayName("รหัสอ้างอิง")]
        public string ReferenceCode { get; set; }
        [DisplayName("ลำดับบล็อก")]
        public int Blockheight { get; set; }
        [DisplayName("รหัสธุรกรรม")]
        public string TxId { get; set; }
      
    }
}

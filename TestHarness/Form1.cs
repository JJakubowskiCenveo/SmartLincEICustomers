using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartLincInterface;
using SAP;

namespace TestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EntityShipment objES = new EntityShipment();
            objES.objDetails = new EntityShipmentDetails();
            objES.objDetails.strDeliveryDocNumber = txtOrder.Text;
            objES.objDetails.strAdditionalIdentifier = txtCustomer.Text;

            EntityConnection objConnection = new EntityConnection();
///            objConnection.strDSNName = @"varic1-sql12c1\erp";
///            objConnection.strDatabase = "PSIntegration";
///            objConnection.strUserID = "service.ps";
///            objConnection.strPassword = "Cen501veo";
///            objES.ToolKit = new EntityToolKit();
            objConnection.strDSNName = @"varic1-sql12c1\erp";
            objConnection.strDatabase = "PSIntegration";
            objConnection.strUserID = "service.ps";
            objConnection.strPassword = "Cen501veo";
            objES.ToolKit = new EntityToolKit();

            objES.ToolKit.objConnection = objConnection;

            SAPInterface objSample = new SAPInterface();
            objSample.Pull(objES);            
        }
    }
}

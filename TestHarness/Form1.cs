using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartLincInterface;
using System.Xml.Serialization;
using System.Xml;
using CMS;
using Lexis;
using LexisGlobal;
using SAP;
using System.IO;

namespace TestHarness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string strInterface = lstInterface.SelectedItem.ToString().Trim();
            EntityShipment objES = new EntityShipment
            {
                objDetails = new EntityShipmentDetails
                {
                    strDeliveryDocNumber = txtOrder.Text
                }
            };

            EntityConnection objConnection = new EntityConnection();
            if (radioButtonDev.Checked == true)
            {
                switch (strInterface.ToUpper())
                {
                    case "CMS":
                        objConnection.strDSNName = "Systema";
                        objConnection.strServer = "SystemA.cadmus.com";
                        objConnection.strDatabase = "SHLIB";
                        objConnection.strUserID = "CENPROUSER";
                        objConnection.strPassword = "c1n9r2u8e3";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "LEXIS":
                        objConnection.strDSNName = @"dvsqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "LEXISGLOBAL":
                        objConnection.strDSNName = @"dvsqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "SAP":
                        objConnection.strDSNName = @"dvsqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    default:
                        MessageBox.Show("Interface was not selected");
                        break;
                }
            }
            if (radioButtonQA.Checked == true)
            {
                switch (strInterface.ToUpper())
                {
                    case "CMS":
                        objConnection.strDSNName = "Sysatr";
                        objConnection.strServer = "Sysatr.cadmus.com";
                        objConnection.strDatabase = "SHLIB";
                        objConnection.strUserID = "ODBCUSER";
                        objConnection.strPassword = "g312h47m63";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "LEXIS":
                        objConnection.strDSNName = @"qasqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "LEXISGLOBAL":
                        objConnection.strDSNName = @"qasqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "SAP":
                        objConnection.strDSNName = @"qasqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    default:
                        MessageBox.Show("Interface was not selected");
                        break;
                }
            }
            if (radioButtonPD.Checked == true)
            {
                switch (strInterface.ToUpper())
                {
                    case "CMS":
                        objConnection.strDSNName = "Systemb";
                        objConnection.strServer = "SystemB.cadmus.com";
                        objConnection.strDatabase = "SHLIB";
                        objConnection.strUserID = "ODBCUSER";
                        objConnection.strPassword = "g312h47m63";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "LEXIS":
                        objConnection.strDSNName = @"pdsqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "LEXISGLOBAL":
                        objConnection.strDSNName = @"pdsqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    case "SAP":
                        objConnection.strDSNName = @"pdsqlmonarch.cenveo.cvo.net";
                        objConnection.strDatabase = "PSIntegration";
                        objConnection.strUserID = "service.ps";
                        objConnection.strPassword = "Cen501veo";
                        objES.ToolKit = new EntityToolKit();
                        break;
                    default:
                        MessageBox.Show("Interface was not selected");
                        break;
                }
            }

            objES.ToolKit.objConnection = objConnection;

            switch (strInterface.ToUpper())
            {
                case "CMS":
                    break;
                case "LEXIS":
                    LexisInterface objSample = new LexisInterface();
                    objSample.Pull(objES);
                    break;
                case "LEXISGLOBAL":
                    LexisGlobalInterface objSampleLG = new LexisGlobalInterface();
                    objSampleLG.Pull(objES);
                    break;
                case "SAP":
                    SAPInterface objSampleSAP = new SAPInterface();
                    objSampleSAP.Pull(objES);
                    break;
                default:
                    MessageBox.Show("Interface was not selected");
                    break; 
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string strInterface = lstInterface.SelectedItem.ToString().Trim();
            EntityShipment objES = new EntityShipment();
            //This to debug the code through xml
            XmlSerializer serializer = new XmlSerializer(typeof(EntityShipment));

            using (TextReader reader = new StringReader(txtXML.Text))
            {
                objES = (EntityShipment)serializer.Deserialize(reader);
            }
            switch (strInterface.ToUpper())
            {
                case "CMS":
                    CMSInterface objCMS = new CMSInterface();
                    objCMS.Putback(objES);
                    break;
                case "LEXIS":
                    LexisInterface objLexis = new LexisInterface();
                    objLexis.Putback(objES);
                    break;
                case "LEXISGLOBAL":
                    LexisGlobalInterface objLexisGlobal = new LexisGlobalInterface();
                    objLexisGlobal.Putback(objES);
                    break;
                case "SAP":
                    SAPInterface objSAP = new SAPInterface();
                    objSAP.Putback(objES);
                    break;
                default:
                    MessageBox.Show("Interface was not selected");
                    break;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string strInterface = lstInterface.SelectedItem.ToString().Trim();
            EntityShipment objES = new EntityShipment();
            //This to debug the code through xml
            XmlSerializer serializer = new XmlSerializer(typeof(EntityShipment));
            //objEIShipment = (EntityShipment)serializer.Deserialize(new XmlTextReader(txtXML.Text));

            using (TextReader reader = new StringReader(txtXML.Text))
            {
                objES = (EntityShipment)serializer.Deserialize(reader);
            }
            switch (strInterface.ToUpper())
            {
                case "CMS":
                    CMSInterface objCMS = new CMSInterface();
                    objCMS.Void(objES);
                    break;
                case "LEXIS":
                    LexisInterface objLexis = new LexisInterface();
                    objLexis.Void(objES);
                    break;
                case "LEXISGLOBAL":
                    LexisGlobalInterface objLexisGlobal = new LexisGlobalInterface();
                    objLexisGlobal.Void(objES);
                    break;
                case "SAP":
                    SAPInterface objSAP = new SAPInterface();
                    objSAP.Void(objES);
                    break;
                default:
                    MessageBox.Show("Interface was not selected");
                    break;
            }
        }
    }
}

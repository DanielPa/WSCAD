using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DipSwitch_VarListGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string macroPath = "../../Macro/";//@"C:\WSCAD\WSCAD SUITE\2017\Macros\";
            string macroName = "DipSwitch";
            string varListFileName = macroPath+macroName+".wsMVL";
            string macroFileName = macroPath+macroName + ".wsELM";
            try
            {
                WS variantList = new WS();//openXml(fileName);
                variantList.Items = new WSVariantList[1];
                variantList.Items[0] = new WSVariantList(); 
                    variantList.Items[0].Variant = new WSVariantListVariant[256];
                
                for (int i = 0; i < 256; i++)
                {
                    WSVariantListVariant variant = new WSVariantListVariant();
                    variant.ID = (i + 1).ToString();
                    variant.Desc = i.ToString("D3");
                    variant.PH = new WSVariantListVariantPH[16];
                    int j = 0;
                    int k = 0;
                    char[] binaryByteChar= (Convert.ToString(i, 2).PadLeft(8, '0')).ToCharArray();
                    foreach (char bit in binaryByteChar)
                    {
                        if (bit.Equals('1'))
                        {
                            variant.PH[k] = new WSVariantListVariantPH() { Name = j.ToString()+"_ON", Value = "n" };
                            variant.PH[k + 1] = new WSVariantListVariantPH() { Name = j.ToString() + "_OFF", Value = " " };
                        }
                        else
                        {
                            variant.PH[k] = new WSVariantListVariantPH() { Name = j.ToString() + "_ON", Value = " " };
                            variant.PH[k + 1] = new WSVariantListVariantPH() { Name = j.ToString() + "_OFF", Value = "n" };
                        }
                        
                        k += 2;
                        j++;
                    }
                    variantList.Items[0].Variant[i] = variant;
                }               
                writeXml(variantList, varListFileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }

        static WS openXml(string _filename)
        {
            StreamReader xmlFileStream = new StreamReader(_filename);
            XmlSerializer wsSerializer = new XmlSerializer(typeof(WS));
            WS returnValue = (WS)wsSerializer.Deserialize(xmlFileStream);
            xmlFileStream.Close();
            return returnValue;
        }

        static void writeXml(WS _variantListe, string _filename)
        {
            XmlSerializer wsSerializer = new XmlSerializer(typeof(WS));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(_filename, settings);           
            wsSerializer.Serialize(writer, _variantListe);
            writer.Flush();
            writer.Close();
        }
    }
}

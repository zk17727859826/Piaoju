using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;

namespace ASP.NETClass
{
    public class WebPrintClass
    {
        public static string TableToXml<T>(IList<T> DtTable)
        {
            
            StringBuilder PrintStr = new StringBuilder();
            XmlWriterSettings setting = new XmlWriterSettings();
            PropertyInfo[] TableColumns;
            Type TableType = DtTable[0].GetType();
            int ColRecn, RowRecn;
            string ColType;

            setting.Indent = true;
            setting.OmitXmlDeclaration = true;
            TableColumns = TableType.GetProperties();


            using (XmlWriter PrintXml = XmlWriter.Create(PrintStr, setting))
            {
                PrintXml.WriteStartElement("xml");
                PrintXml.WriteAttributeString("xmlns", "s", null, "u");
                PrintXml.WriteAttributeString("xmlns", "dt", null, "u");
                PrintXml.WriteAttributeString("xmlns", "rs", null, "u");
                PrintXml.WriteAttributeString("xmlns", "z", null, "#R");
                PrintXml.WriteStartElement("s", "Schema", null);
                PrintXml.WriteAttributeString("id", "RowsetSchema");
                PrintXml.WriteStartElement("s", "ElementType", null);
                PrintXml.WriteAttributeString("name", "row");
                PrintXml.WriteAttributeString("content", "eltOnly");
                PrintXml.WriteAttributeString("rs", "updatable", null, "true");

                for (ColRecn = 0; ColRecn < TableColumns.Length; ColRecn++)
                {
                    ColType = TableColumns[ColRecn].PropertyType.ToString();
                    if (ColType == "System.String" || ColType == "System.DateTime"
                        || ColType == "System.Int" || ColType == "System.Double"
                        || ColType == "System.Boolean" || ColType == "System.Decimal")
                    {
                        PrintXml.WriteStartElement("s", "AttributeType", null);
                        PrintXml.WriteAttributeString("name", TableColumns[ColRecn].Name);

                        PrintXml.WriteAttributeString("rs", "number", null, (ColRecn + 1).ToString());
                        PrintXml.WriteAttributeString("rs", "nullable", null, "true");
                        PrintXml.WriteAttributeString("rs", "maydefer", null, "true");
                        PrintXml.WriteAttributeString("rs", "writeunknown", null, "true");
                        PrintXml.WriteAttributeString("rs", "basetable", null, "");
                        PrintXml.WriteAttributeString("rs", "basecolumn", null, TableColumns[ColRecn].Name);

                        if (ColType == "System.String")
                        {  //char
                            PrintXml.WriteStartElement("s", "datatype", null);
                            PrintXml.WriteAttributeString("dt", "type", null, "string");
                            PrintXml.WriteAttributeString("dt", "maxLength", null, "8000");
                            PrintXml.WriteEndElement();
                        }
                        else if (ColType == "System.DateTime")
                        {  //dateTime
                            PrintXml.WriteStartElement("s", "datatype", null);
                            PrintXml.WriteAttributeString("dt", "type", null, "dateTime");
                            PrintXml.WriteAttributeString("dt", "maxLength", null, "16");
                            PrintXml.WriteEndElement();
                        }
                        else if (ColType == "System.Double")
                        {  //Float
                            PrintXml.WriteStartElement("s", "datatype", null);
                            PrintXml.WriteAttributeString("dt", "type", null, "float");
                            PrintXml.WriteAttributeString("dt", "maxLength", null, "8");
                            PrintXml.WriteEndElement();
                        }
                        else if (ColType == "System.Decimal")
                        {
                            PrintXml.WriteStartElement("s", "datatype", null);
                            PrintXml.WriteAttributeString("dt", "type", null, "decimal");
                            PrintXml.WriteAttributeString("dt", "maxLength", null, "8");
                            PrintXml.WriteEndElement();
                        }
                        else if (ColType == "System.Int")
                        {  //int
                            PrintXml.WriteStartElement("s", "datatype", null);
                            PrintXml.WriteAttributeString("dt", "type", null, "int");
                            PrintXml.WriteAttributeString("dt", "maxLength", null, "4");
                            PrintXml.WriteEndElement();
                        }
                        else if (ColType == "System.Boolean")
                        {  //bit
                            PrintXml.WriteStartElement("s", "datatype", null);
                            PrintXml.WriteAttributeString("dt", "type", null, "boolean");
                            PrintXml.WriteAttributeString("dt", "maxLength", null, "2");
                            PrintXml.WriteEndElement();
                        }
                        PrintXml.WriteEndElement();
                    }
                }
                PrintXml.WriteEndElement();
                PrintXml.WriteEndElement();


                PrintXml.WriteStartElement("rs", "data", null);
                for (RowRecn = 0; RowRecn < DtTable.Count; RowRecn++)
                {
                    PrintXml.WriteStartElement("z", "row", null);
                    for (ColRecn = 0; ColRecn < TableColumns.Length; ColRecn++)
                    {
                        if (Convert.IsDBNull(TableColumns[ColRecn].GetValue(DtTable[RowRecn], null)) == true)
                            continue;

                        ColType = TableColumns[ColRecn].PropertyType.ToString();
                        if (ColType == "System.String" || ColType == "System.DateTime"
                            || ColType == "System.Int" || ColType == "System.Double"
                            || ColType == "System.Boolean" || ColType == "System.Decimal")
                        {
                            if (ColType == "System.DateTime")
                            {
                                DateTime TmpDateTime = System.Convert.ToDateTime(TableColumns[ColRecn].GetValue(DtTable[RowRecn], null));
                                string TmpStr = TmpDateTime.ToString("yyyy-MM-dd") + "T" + TmpDateTime.ToString("HH:mm:ss");
                                PrintXml.WriteAttributeString(TableColumns[ColRecn].Name, TmpStr);
                            }
                            else
                            {
                                PrintXml.WriteAttributeString(TableColumns[ColRecn].Name, TableColumns[ColRecn].GetValue(DtTable[RowRecn], null).ToString());
                            }
                        }
                    }
                    PrintXml.WriteEndElement();
                }
                PrintXml.WriteEndElement();

                PrintXml.WriteEndElement();
            }

            return PrintStr.ToString();
        }


        public static string FileToString(string FileName)
        {
            string[] ByteStr = new string[256];
            for (int i = 0; i < 256; i++)
            {
                if (i == 0)
                    ByteStr[i] = "00";
                else if (i < 16)
                    ByteStr[i] = "0" + Convert.ToString(i, 16).ToUpper();
                else
                    ByteStr[i] = Convert.ToString(i, 16).ToUpper();
            }

            if (File.Exists(FileName) == false)
                return "";

            byte[] FileValue = File.ReadAllBytes(FileName);
            StringBuilder FileStr = new StringBuilder();

            for (int FileRecn = 0; FileRecn < FileValue.Length; FileRecn++)
            {
                FileStr.Append(ByteStr[FileValue[FileRecn]]);
            }
            FileValue = null;

            return FileStr.ToString();
        }

        public static string TablePictureToStr<T>(IList<T> DtTable, string PictureFieldName, string PictureFilePath)
        {
            string ScriptPicture = "";
            string FieldValue, PictureName, PictureValue;
            PropertyInfo[] TableColumns;
            Type TableType = DtTable[0].GetType();
            int ColRecn, RowRecn;
            int PictureFieldCol = 0;

            TableColumns = TableType.GetProperties();
            for (ColRecn = 0; ColRecn < TableColumns.Length; ColRecn++)
            {
                if (PictureFieldName.Trim() == TableColumns[ColRecn].Name.Trim())
                {
                    PictureFieldCol = ColRecn;
                    break;
                }
            }


            for (RowRecn = 0; RowRecn < DtTable.Count; RowRecn++)
            {

                FieldValue = TableColumns[PictureFieldCol].GetValue(DtTable[RowRecn], null).ToString();
                if (FieldValue == "")
                    continue;

                PictureName = PictureFilePath + FieldValue;
                if (File.Exists(PictureName) == false)
                    continue;

                PictureValue = FileToString(PictureName);

                ScriptPicture = ScriptPicture + "ObjPrintMange.SavePictureFile('" + FieldValue + "' , '" + PictureValue + "' );";  //读取图片文件至打印控件，参数：文件名，该文件内容
            }
            return ScriptPicture;
        }


    }
}

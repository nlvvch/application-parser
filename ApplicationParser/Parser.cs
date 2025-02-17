﻿using System.Collections.Generic;
using System.Xml;

namespace ApplicationParser
{
    public class Parser
    {
        public Application Parse(string xml)
        {
            var xmlDoc = new XmlDocument(); // Create an XML document object
            xmlDoc.LoadXml(xml);
            var app = new Application();
            app.Guid = xmlDoc.SelectNodes("Application").Item(0).SelectNodes("Guid").Item(0).InnerText;
            app.Name = xmlDoc.SelectNodes("Application").Item(0).SelectNodes("Name").Item(0).InnerText;
            var objList = new List<ObjectDef>();
            var objects = xmlDoc.GetElementsByTagName("Object");
            foreach (XmlNode obj in objects)
            {
                var objDef = ParseObject(obj);
                objList.Add(objDef);
            }
            var tabList = new List<Tab>();
            var tabs = xmlDoc.GetElementsByTagName("Tab");
            foreach (XmlNode obj in tabs)
            {
                var objDef = ParseNode<Tab>(obj);
                tabList.Add(objDef);
            }
            app.Objects = objList;
            app.Tabs = tabList;
            return app;
        }
        public ObjectDef ParseObject(XmlNode node)
        {
            var obj = new ObjectDef();
            obj.Name = node.SelectNodes("Name").Item(0).InnerText;
            obj.Guid = node.SelectNodes("Guid").Item(0).InnerText;
            var fields = node.SelectNodes("Fields")
                .Item(0)
                .SelectNodes("Field");
            var systemFields = node.SelectNodes("SystemFields")
                .Item(0)
                .SelectNodes("SystemField");
            var fieldList = new List<Field>();
            foreach (XmlNode field in fields)
            {
                var fieldDef = ParseField(field);
                if (fieldDef != null)
                {
                    fieldList.Add(fieldDef);
                }
            }
            foreach (XmlNode field in systemFields)
            {
                var fieldDef = ParseField(field, true);
                if (fieldDef != null)
                {
                    fieldList.Add(fieldDef);
                }
            }
            obj.Fields = fieldList;
            return obj;
        }

        private Field ParseField(XmlNode field, bool system = false)
        {
            var guid = field.SelectNodes("Guid").Item(0);
            var name = field.SelectNodes("DisplayName").Item(0);
            var fieldId = int.Parse(field.SelectNodes("FieldTypeId").Item(0).InnerText);
            var artifact = new Field
            {
                Guid = guid.InnerText,
                Name = name.InnerText,
                FieldTypeId = fieldId
            };
            if (artifact.Name.Contains("System"))
            {
                return null;
            }
            var choiceList = new List<ArtifactDef>();
            var codes = field.SelectNodes("Codes").Item(0).SelectNodes("Code");
            foreach (XmlNode code in codes)
            {
                var choiceDef = ParseNode<Field>(code);
                choiceList.Add(choiceDef);
            }
            artifact.Choices = choiceList;
            return artifact;
        }
        private T ParseNode<T>(XmlNode node) where T : ArtifactDef, new()
        {
            var guid = node.SelectNodes("Guid").Item(0);
            var name = node.SelectNodes("Name").Item(0);
            var artifact = new T { Guid = guid.InnerText, Name = name.InnerText };
            return artifact;
        }
    }
}

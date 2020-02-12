﻿using System;
using System.Linq;
using System.Text;
using Vs.Graph.Core.Data;
using Vs.Graph.Core.Data.Exceptions;

namespace Vs.DataProvider.MsSqlGraph
{
    public class AttributeSchemaScript : IScriptable<Attributes>
    {
        public string CreateScript(Attributes @object)
        {
            if (@object == null) return string.Empty;
            var sb = new StringBuilder();

            var typesWithMyAttribute =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(AttributeTypeAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<AttributeTypeAttribute>() };

            foreach (var attribute in @object)
            {
                // Resolve AttributeType from types that inherit from IAttributeType
                switch (((AttributeTypeAttribute)attribute.Type.GetType().GetCustomAttributes(typeof(AttributeTypeAttribute), true)[0]).Name)
                {
                    case "datum":
                        sb.Append($"{attribute.Name} DATETIME,");
                        break;
                    case "elfproef":
                        sb.Append($"{attribute.Name} VARCHAR(10),");
                        break;
                    case "euro":
                        sb.Append($"{attribute.Name} DECIMAL,");
                        break;
                    case "periode":
                        sb.Append($"{attribute.Name}_begin DATETIME,");
                        sb.Append($"{attribute.Name}_eind  DATETIME,");
                        break;
                    case "Text":
                        sb.Append($"{attribute.Name}  NTEXT,");
                        break;
                    default:
                        throw new AttributeNotSupportedException();
                }
            }
            return sb.ToString().TrimEnd(',') + ' ';
        }
    }
}

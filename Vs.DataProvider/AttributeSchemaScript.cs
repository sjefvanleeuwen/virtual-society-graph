using System;
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
            foreach (var attribute in @object)
            {
                var msSqlField = "";
                switch (attribute.Type)
                {
                    case AttributeType.Currency:
                        msSqlField = "MONEY";
                        break;
                    case AttributeType.DateTime:
                        msSqlField = "DATETIME";
                        break;
                    case AttributeType.Decimal:
                        msSqlField = "DECIMAL";
                        break;
                    case AttributeType.Integer:
                        msSqlField = "INTEGER";
                        break;
                    case AttributeType.Text:
                        msSqlField = "NTEXT";
                        break;
                    default:
                        throw new AttributeNotSupportedException();
                }
                sb.AppendLine($"{attribute.Name} {msSqlField}, ");
            }
            return sb.ToString();
        }
    }
}

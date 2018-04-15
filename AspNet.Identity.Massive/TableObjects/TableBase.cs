using Massive;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace AspNet.Identity.Massive
{
    public abstract class TableBase : DynamicModel
    {
        public TableBase(string connStringName, string tableName)
            : base(connStringName, tableName, "Id")
        {
        }

        public override dynamic Insert(object o)
        {
            var oAsExpando = o.ToExpando();
            var oAsDictionary = (IDictionary<string, object>)oAsExpando;
            oAsDictionary.Remove(PrimaryKeyField);
            dynamic result = base.Insert(oAsDictionary);
            o.GetType().GetProperty(PrimaryKeyField).SetValue(o, oAsDictionary[PrimaryKeyField]);
            return result;
        }
    }
}
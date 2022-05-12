// This file was auto-generated by ML.NET Model Builder. 

using Microsoft.ML.Data;

namespace HRML.Model
{
    public class ModelInput
    {
        [ColumnName("Id"), LoadColumn(0)]
        public float Id { get; set; }


        [ColumnName("Name"), LoadColumn(1)]
        public string Name { get; set; }


        [ColumnName("BirthDate"), LoadColumn(2)]
        public string BirthDate { get; set; }


        [ColumnName("name_function"), LoadColumn(3)]
        public string Name_function { get; set; }


        [ColumnName("name_department"), LoadColumn(4)]
        public string Name_department { get; set; }


        [ColumnName("Studies"), LoadColumn(5)]
        public string Studies { get; set; }


        [ColumnName("Experience"), LoadColumn(6)]
        public string Experience { get; set; }


        [ColumnName("Observation"), LoadColumn(7)]
        public string Observation { get; set; }


        [ColumnName("ModeApply"), LoadColumn(8)]
        public float ModeApply { get; set; }


        [ColumnName("OffertStatus"), LoadColumn(9)]
        public string OffertStatus { get; set; }


    }
}

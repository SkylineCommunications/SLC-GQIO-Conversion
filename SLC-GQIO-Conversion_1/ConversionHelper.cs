namespace Conversion
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Skyline.DataMiner.Analytics.GenericInterface;

    public class ConversionHelper
    {
        public static readonly Dictionary<string, List<string>> SupportedConversions = new Dictionary<string, List<string>>
        {
            { "String", new List<string> { "Int", "DateTime", "Boolean", "Double", "TimeSpan" } },
            { "Int", new List<string> { "String", "DateTime", "Boolean", "Double" } },
            { "DateTime", new List<string> { "String", "Double", "TimeSpan" } },
            { "Boolean", new List<string> { "String", "Int" } },
            { "Double", new List<string> { "String", "Int", "Boolean", "DateTime" } },
            { "TimeSpan", new List<string> { "String" } },
        };

        private readonly GQIColumn inputColumn;
        private readonly GQIColumnType newColumnType;
        private readonly string newColumnName;
        private readonly string exceptionValue;

        private GQIColumn newColumn;

        public ConversionHelper(GQIColumn inputColumn, GQIColumnType newColumnType, string newColumnName, string exceptionValue)
        {
            this.inputColumn = inputColumn;
            this.newColumnType = newColumnType;
            this.newColumnName = newColumnName;
            this.exceptionValue = exceptionValue;
        }

        public GQIColumn CreateNewColumn()
        {
            switch (newColumnType)
            {
                case GQIColumnType.String:
                    newColumn = new GQIStringColumn(newColumnName);
                    return newColumn;

                case GQIColumnType.Int:
                    newColumn = new GQIIntColumn(newColumnName);
                    return newColumn;

                case GQIColumnType.DateTime:
                    newColumn = new GQIDateTimeColumn(newColumnName);
                    return newColumn;

                case GQIColumnType.Boolean:
                    newColumn = new GQIBooleanColumn(newColumnName);
                    return newColumn;

                case GQIColumnType.Double:
                    newColumn = new GQIDoubleColumn(newColumnName);
                    return newColumn;

                case GQIColumnType.TimeSpan:
                    newColumn = new GQITimeSpanColumn(newColumnName);
                    return newColumn;

                default:
                    // No Action
                    break;
            }

            throw new GenIfException($"Column type not supported: {newColumnType}");
        }

        public void SetNewRowValue(GQIEditableRow row)
        {
            var fromType = inputColumn.Type;
            switch (fromType)
            {
                case GQIColumnType.String:
                    var rowStringValue = row.GetValue<string>(inputColumn);
                    ConvertFromString(row, newColumn.Type, rowStringValue);
                    break;

                case GQIColumnType.Int:
                    var rowIntValue = row.GetValue<int>(inputColumn);
                    ConvertFromInt(row, newColumn.Type, rowIntValue);
                    break;

                case GQIColumnType.DateTime:
                    var rowDateTimeValue = row.GetValue<DateTime>(inputColumn);
                    ConvertFromDateTime(row, newColumn.Type, rowDateTimeValue);
                    break;

                case GQIColumnType.Boolean:
                    var rowBoolValue = row.GetValue<bool>(inputColumn);
                    ConvertFromBoolean(row, newColumn.Type, rowBoolValue);
                    break;

                case GQIColumnType.Double:
                    var rowDoubleValue = row.GetValue<double>(inputColumn);
                    ConvertFromDouble(row, newColumn.Type, rowDoubleValue);
                    break;

                case GQIColumnType.TimeSpan:
                    var rowTimeSpanValue = row.GetValue<TimeSpan>(inputColumn);
                    ConvertFromTimeSpan(row, newColumn.Type, rowTimeSpanValue);
                    break;

                default:
                    throw new GenIfException($"Conversion from {fromType} is not supported.");
            }
        }

        #region Conversions
        private void ConvertFromString(GQIEditableRow row, GQIColumnType toType, string inputValue)
        {
            switch (toType)
            {
                case GQIColumnType.Int:
                    if (int.TryParse(inputValue, out var intValue) && intValue != int.MinValue)
                    {
                        row.SetValue(newColumn, intValue);
                    }
                    else
                    {
                        row.SetValue(newColumn, int.MinValue, exceptionValue);
                    }

                    break;
                case GQIColumnType.DateTime:
                    if (DateTime.TryParse(inputValue, out DateTime dateTimeValue) && dateTimeValue != DateTime.MinValue)
                    {
                        row.SetValue(newColumn, dateTimeValue.ToUniversalTime());
                    }
                    else
                    {
                        row.SetValue(newColumn, DateTime.MinValue.ToUniversalTime(), exceptionValue);
                    }

                    break;
                case GQIColumnType.Boolean:
                    if (bool.TryParse(inputValue, out var boolValue))
                    {
                        row.SetValue(newColumn, boolValue);
                    }
                    else
                    {
                        row.SetValue(newColumn, null, exceptionValue);
                    }

                    break;
                case GQIColumnType.Double:
                    if (double.TryParse(inputValue, out var doubleValue) && doubleValue != double.MinValue)
                    {
                        row.SetValue(newColumn, doubleValue);
                    }
                    else
                    {
                        row.SetValue(newColumn, double.MinValue, exceptionValue);
                    }

                    break;
                case GQIColumnType.TimeSpan:
                    if (TimeSpan.TryParse(inputValue, out var timeSpanValue) && timeSpanValue != TimeSpan.Zero)
                    {
                        row.SetValue(newColumn, timeSpanValue);
                    }
                    else
                    {
                        row.SetValue(newColumn, TimeSpan.Zero, exceptionValue);
                    }

                    break;
                default:
                    throw new GenIfException($"Conversion from String to {toType} is not supported.");
            }
        }

        private void ConvertFromInt(GQIEditableRow row, GQIColumnType toType, int input)
        {
            switch (toType)
            {
                case GQIColumnType.String:
                    row.SetValue(newColumn, input.ToString());
                    break;
                case GQIColumnType.DateTime:
                    if (DateTime.TryParse(input.ToString(), out var dateTimeValue) && dateTimeValue != DateTime.MinValue)
                    {
                        row.SetValue(newColumn, dateTimeValue.ToUniversalTime());
                    }
                    else
                    {
                        row.SetValue(newColumn, DateTime.MinValue.ToUniversalTime(), exceptionValue);
                    }

                    break;
                case GQIColumnType.Boolean:
                    row.SetValue(newColumn, input != 0);
                    break;
                case GQIColumnType.Double:
                    row.SetValue(newColumn, Convert.ToDouble(input));
                    break;
                default:
                    throw new GenIfException($"Conversion from Int to {toType} is not supported.");
            }
        }

        private void ConvertFromDateTime(GQIEditableRow row, GQIColumnType toType, DateTime input)
        {
            switch (toType)
            {
                case GQIColumnType.String:
                    row.SetValue(newColumn, input.ToString(CultureInfo.CurrentCulture));
                    break;
                case GQIColumnType.Double:
                    row.SetValue(newColumn, input.ToOADate());
                    break;
                case GQIColumnType.TimeSpan:
                    row.SetValue(newColumn, input.TimeOfDay);
                    break;
                default:
                    throw new GenIfException($"Conversion from DateTime to {toType} is not supported.");
            }
        }

        private void ConvertFromBoolean(GQIEditableRow row, GQIColumnType toType, bool input)
        {
            switch (toType)
            {
                case GQIColumnType.Int:
                    row.SetValue(newColumn, input ? 1 : 0);
                    break;
                case GQIColumnType.String:
                    row.SetValue(newColumn, input.ToString());
                    break;
                default:
                    throw new GenIfException($"Conversion from Boolean to {toType} is not supported.");
            }
        }

        private void ConvertFromDouble(GQIEditableRow row, GQIColumnType toType, double input)
        {
            switch (toType)
            {
                case GQIColumnType.Int:
                    row.SetValue(newColumn, Convert.ToInt32(input));
                    break;
                case GQIColumnType.String:
                    row.SetValue(newColumn, input.ToString(CultureInfo.CurrentCulture));
                    break;
                case GQIColumnType.Boolean:
                    row.SetValue(newColumn, input != 0.0);
                    break;
                case GQIColumnType.DateTime:
                    if (DateTime.FromOADate(input) != DateTime.MinValue)
                    {
                        row.SetValue(newColumn, DateTime.FromOADate(input).ToUniversalTime());
                    }
                    else
                    {
                        row.SetValue(newColumn, DateTime.MinValue.ToUniversalTime(), exceptionValue);
                    }

                    break;
                default:
                    throw new GenIfException($"Conversion from Double to {toType} is not supported.");
            }
        }

        private void ConvertFromTimeSpan(GQIEditableRow row, GQIColumnType type, TimeSpan rowTimeSpanValue)
        {
            row.SetValue(newColumn, rowTimeSpanValue.ToString());
        }

        #endregion
    }
}
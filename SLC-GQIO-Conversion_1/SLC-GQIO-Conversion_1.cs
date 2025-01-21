namespace Conversion
{
    using System;

    using Skyline.DataMiner.Analytics.GenericInterface;
    using Skyline.DataMiner.Net.Helper;

    [GQIMetaData(Name = "Convert")]
    public class Conversion : IGQIRowOperator, IGQIInputArguments, IGQIColumnOperator
    {
        private readonly GQIColumnDropdownArgument _inputColumnArg = new GQIColumnDropdownArgument("Column") { IsRequired = true };
        private readonly GQIStringArgument _exceptionValueArg = new GQIStringArgument("Exception Value") { IsRequired = false, DefaultValue = "N/A" };
        private readonly GQIStringArgument _newColumnNameArg = new GQIStringArgument("New Column Name") { IsRequired = false };

        private GQIStringDropdownArgument _convertToArg;
        private string _newColumnName;
        private GQIColumn _newColumn;
        private string _converTo;
        private GQIColumn _inputColumn;
        private string _exceptionValue;
        private ConversionHelper conversionHelper;

        public GQIArgument[] GetInputArguments()
        {
            var typeOptions = new[]
            {
                GQIColumnType.Double.ToString(),
                GQIColumnType.Int.ToString(),
                GQIColumnType.String.ToString(),
                GQIColumnType.Boolean.ToString(),
                GQIColumnType.TimeSpan.ToString(),
                GQIColumnType.DateTime.ToString(),
            };

            _convertToArg = new GQIStringDropdownArgument("Convert To", typeOptions)
            {
                IsRequired = true,
            };

            return new GQIArgument[] { _inputColumnArg, _convertToArg, _newColumnNameArg, _exceptionValueArg };
        }

        public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
        {
            _exceptionValue = args.GetArgumentValue(_exceptionValueArg);

            if (_exceptionValue.IsNullOrEmpty())
            {
                _exceptionValue = _exceptionValueArg.DefaultValue;
            }

            _inputColumn = args.GetArgumentValue(_inputColumnArg);
            _converTo = args.GetArgumentValue(_convertToArg);

            _newColumnName = args.GetArgumentValue(_newColumnNameArg);

            if (_newColumnName.IsNullOrEmpty())
            {
                _newColumnName = $"{_inputColumn.Name} (as {_converTo})";
            }

            var isConversionSupported = IsConversionSupported(_inputColumn.Type.ToString(), _converTo);

            if (!isConversionSupported)
            {
                throw new GenIfException($"Cannot Convert {_inputColumn.Type} to {_converTo}");
            }

            return new OnArgumentsProcessedOutputArgs();
        }

        public void HandleColumns(GQIEditableHeader header)
        {
            if (!Enum.TryParse(_converTo, true, out GQIColumnType newColumnType))
            {
                throw new GenIfException($"Unable to parse {_converTo}");
            }

            conversionHelper = new ConversionHelper(_inputColumn, newColumnType, _newColumnName, _exceptionValue);

            _newColumn = conversionHelper.CreateNewColumn();

            if (_newColumn != null)
                header.AddColumns(_newColumn);
        }

        public void HandleRow(GQIEditableRow row)
        {
            if (_inputColumn == null || _newColumn == null)
                return;

            conversionHelper.SetNewRowValue(row);
        }

        private static bool IsConversionSupported(string inputType, string targetType)
        {
            if (ConversionHelper.SupportedConversions.TryGetValue(inputType, out var allowedConversions))
            {
                return allowedConversions.Contains(targetType);
            }

            return false;
        }
    }
}
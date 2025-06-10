# Data Transformer

## About

This package provides a **custom operator** designed for seamless type conversions across multiple data formats. Whether you're working with strings, numbers, dates, or boolean values, this tool will ensure data consistency and flexibility.

## Key Features

- **Versatile type conversions** – Effortlessly convert between various data types:

  - **String** ↔ Int, DateTime, Boolean, Double, TimeSpan
  - **Int** ↔ String, DateTime, Boolean, Double
  - **DateTime** ↔ String, Double, TimeSpan
  - **Boolean** ↔ String, Int
  - **Double** ↔ String, Int, Boolean, DateTime
  - **TimeSpan** ↔ String

- **Customizable exception handling** – Define default values for invalid conversions.
  
  If none are specified, the default value will be *N/A*.

- **Dynamic column addition** – Automatically generates new columns with the converted data type.

  If no column name is provided, the default format will be *Column Name (as Type)*. Example: `My Column (as double)`

## Technical Reference

For **technical details**, advanced configuration, and troubleshooting, see the **full documentation** of each script:

- 📄 [SLC-GQIO-Conversion Readme](https://github.com/SkylineCommunications/SLC-GQIO-Conversion/blob/master/SLC-GQIO-Conversion_1/README.md)

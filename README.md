# Data Transformer

This package provides a **Ad hoc custom operator** designed for seamless type conversions across multiple data formats. Whether you're working with strings, numbers, dates, or boolean values, this tool ensures data consistency and flexibility.

## Key Features

🔹 **Versatile Type Conversions** – Effortlessly convert between various data types:
  - **String** ↔ Int, DateTime, Boolean, Double, TimeSpan
  - **Int** ↔ String, DateTime, Boolean, Double
  - **DateTime** ↔ String, Double, TimeSpan
  - **Boolean** ↔ String, Int
  - **Double** ↔ String, Int, Boolean, DateTime
  - **TimeSpan** ↔ String

🔹 **Customizable Exception Handling** – Define default values for invalid conversions. If none are specified, the default value will be *N/A*.

🔹 **Dynamic Column Addition** – Automatically generates new columns with the converted data type.
  - If no column name is provided, the default format will be *Column Name (as Type)* (e.g., `My Column (as double)`).

For **technical details**, advanced configuration, and troubleshooting, refer to the **full documentation** for each script:

- 📄 [SLC-GQIO-Conversion Readme](https://github.com/SkylineCommunications/SLC-GQIO-Conversion/blob/master/SLC-GQIO-Conversion_1/README.md)

# SLC-GQIO-Conversion

This repository contains a GQI custom operator, designed to facilitate type conversions. These tools provide robust support for converting data columns between multiple types while maintaining flexibility and configurability.

## Features

- **Supported Type Conversions:**
  - String ↔ Int, DateTime, Boolean, Double, TimeSpan
  - Int ↔ String, DateTime, Boolean, Double
  - DateTime ↔ String, Double, TimeSpan
  - Boolean ↔ String, Int
  - Double ↔ String, Int, Boolean, DateTime
  - TimeSpan ↔ String

- **Customizable Exception Handling:**
  - Specify default values for invalid conversions.
  - If no value is specified the default would be *N/A*
  
- **Dynamic Column Addition:**
  - Automatically creates and adds new columns with the converted data type.
  - If no name is specified for the new column, the new name will be shown as *Column Name* (as *type*) -> e.g My Column (as double)

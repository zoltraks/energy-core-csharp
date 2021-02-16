CHANGES
=======

## 20.12.14 ##

Energy.Base.Text.Trim(IList<>)

Energy.Base.Text.RemoveEmptyElements

## 20.12.13 ##

Energy.Base.Text.IncludeTrailing

Energy.Base.Text.IncludeLeading

Energy.Base.Text.Trim for lists and arrays. Regular Trim will no longer ignore null character (code 0).

## 20.12.11 ##

First published build for Compact Framework.

## 20.12.10 ## 

Fix in Energy.Base.Hex.IsHex.

## 18.12.15 ##

Energy.Base.Cast.BoolToString will always return at least 1 character string (" " for false values when BooleanStyle set to **BooleanStyle.X** or **BooleanStyle.V**).

Use **Trim** if you want to have previous behaviour.

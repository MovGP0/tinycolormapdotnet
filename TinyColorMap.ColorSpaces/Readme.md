# Color Space

Provides methods to convert colors between different color spaces. The following color spaces are supported:

| Color Space | Usage                                                                                                                                                                                                                    |
|-------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| HSL         | HSL (Hue, Saturation, Lightness) is often used in graphics applications and image editing software to select and manipulate colors in a more intuitive way than RGB.                                                     |
| LCH         | (HCL) LCH (Lightness, Chroma, Hue), also known as HCL, is used in perceptual color spaces to make colors appear more uniform to human vision, making it useful in color differentiation and accessibility applications.  |
| RGB         | RGB (Red, Green, Blue) is used in electronic displays, digital cameras, and web design, representing colors as combinations of these three primary colors.                                                               |
| XYZ         | The CIE XYZ color model is a standard reference model for colorimetry, used in various industries to ensure consistent color reproduction across different devices and media.                                            |
| LUV         | The CIE LUV color space is used in colorimetry and for measuring perceived color differences, often applied in industries requiring precise color matching and quality control.                                          |
| HWB         | HWB (Hue, Whiteness, Blackness) is used in some graphic design applications as an alternative to HSL/HSV for more intuitive color adjustments based on adding white or black.                                            |
| CMY         | CMY (Cyan, Magenta, Yellow) is primarily used in color printing, where these colors are combined to produce a wide range of colors.                                                                                      |
| CMYK        | CMYK (Cyan, Magenta, Yellow, Key/Black) is used in color printing processes to create full-color images by combining these four inks.                                                                                    |
| LAB         | The CIE Lab color space is used in various applications including image processing and color correction, where a perceptually uniform color space is beneficial.                                                         |
| YUV         | YUV is used in video compression and transmission, separating the luminance (Y) from the chrominance (U and V) to reduce bandwidth while preserving color information.                                                   |
| YCbCr       | YCbCr is used in digital video and image compression formats like JPEG and MPEG, where separating luminance and chrominance allows for more efficient encoding.                                                          |
| HSV         | HSV (Hue, Saturation, Value) is used in many applications where color description is key, such as image editing and graphic design, providing a more intuitive way to adjust colors.                                     |

## Attribution

The following projects were (partially) used as references for the implementation of the color spaces:

- [HSLuv Color Gradient](https://github.com/adammaj1/hsluv-color-gradient)
- [Colormath](https://github.com/ajalt/colormath)

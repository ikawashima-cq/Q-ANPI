# For more information about using CMake with Android Studio, read the
# documentation: https://d.android.com/studio/projects/add-native-code.html

# Sets the minimum version of CMake required to build the native library.

cmake_minimum_required(VERSION 3.4.1)

# Creates and names a library, sets it as either STATIC
# or SHARED, and provides the relative paths to its source code.
# You can define multiple libraries, and CMake builds them for you.
# Gradle automatically packages shared libraries with your APK.

# Spesifies a path to native header files
include_directories(./)

add_library(
                jni_blowfish
                SHARED
                jni_blowfish.cpp
           )
add_library(
                blowfish
                SHARED
                blowfish.cpp
            )

set( LIB_ROOT ${CMAKE_CURRENT_SOURCE_DIR}/../.. )
set( OUTPUT_DIR ${LIB_ROOT}/lib/${ANDROID_ABI} )

set_target_properties(jni_blowfish PROPERTIES LIBRARY_OUTPUT_DIRECTORY ${OUTPUT_DIR} )

target_link_libraries(jni_blowfish blowfish)

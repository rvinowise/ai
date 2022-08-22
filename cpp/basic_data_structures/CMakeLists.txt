cmake_minimum_required(VERSION 3.11-3.18)

project(
    basic_data_structures
    VERSION 1.0
    LANGUAGES CXX
)

configure_file(config.h.in config.h)


add_library(
    basic_data_structures STATIC 
    src/Figure/Appearance/Appearance.cpp
    src/Figure/Edge/Edge.cpp
    src/Figure/Interval/Interval.hpp
    src/Figure/Subfigure/Subfigure.cpp
    src/Figure/overview/Figure.cpp
)

target_include_directories(${PROJECT_NAME} PUBLIC 
    ${PROJECT_SOURCE_DIR}/src/
)

target_include_directories(${PROJECT_NAME} PUBLIC
    ${PROJECT_BINARY_DIR}
)
    
target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)
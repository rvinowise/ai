cmake_minimum_required(VERSION 3.11-3.18)

project(
    basic_data_structures
    VERSION 1.0
    LANGUAGES CXX
)


configure_file(config.h.in config.h)


add_library(
    basic_data_structures STATIC 
    src/Figure/Figure_appearance/Figure_appearance.cpp
    src/Figure/Edge/Edge.cpp
    src/Interval/Interval.cpp
    src/Figure/Subfigure/Subfigure.cpp
    src/Figure/overview/Figure.cpp
)

target_include_directories(${PROJECT_NAME} PUBLIC 
    ${PROJECT_SOURCE_DIR}/src/
)

target_include_directories(${PROJECT_NAME} PUBLIC
    ${PROJECT_BINARY_DIR}
)
    
#target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)
set_property(TARGET ${PROJECT_NAME} PROPERTY CXX_STANDARD 20)
#set(CMAKE_CXX_STANDARD 20)

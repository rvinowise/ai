set(AI_ROOT "C:/prj/ai/")

cmake_minimum_required(VERSION 3.11-3.18)

add_executable(${PROJECT_NAME}_test
    catch2/finding_sequences.cpp
)

target_include_directories(
    ${PROJECT_NAME} PUBLIC 
    ${PROJECT_SOURCE_DIR}/include
    ${PROJECT_SOURCE_DIR}/src
)

set_property(TARGET ${PROJECT_NAME} PROPERTY CXX_STANDARD 20)
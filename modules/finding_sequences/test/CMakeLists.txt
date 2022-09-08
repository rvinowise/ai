set(AI_ROOT "C:/prj/ai/")

cmake_minimum_required(VERSION 3.11-3.18)

project(finding_sequences_test)


add_executable(${PROJECT_NAME} 
    #src/Finding_sequences/test/boost_ut/finding_sequences_test.cpp
    ../src/Finding_sequences/test/catch2/finding_sequences_test.cpp
    
)


add_subdirectory(
    ../src
    tested_project
)


set_property(TARGET ${PROJECT_NAME} PROPERTY CXX_STANDARD 20)

enable_testing()
add_test(NAME ${PROJECT_NAME} COMMAND ${PROJECT_NAME})

cmake_minimum_required(VERSION 3.18)

project(
    finding_sequences
    VERSION 1
    LANGUAGES CXX
)
set(CMAKE_CXX_STANDARD 20)



add_subdirectory(src)
add_subdirectory(test)


enable_testing()
add_test(NAME test1 COMMAND $<TARGET_FILE:finding_sequences_test>)
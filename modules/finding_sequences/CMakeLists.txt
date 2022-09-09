
cmake_minimum_required(VERSION 3.11-3.18)

project(
    finding_sequences
    VERSION 1
    LANGUAGES CXX
)
set(CMAKE_CXX_STANDARD 20)

include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup()



add_subdirectory(src)
add_subdirectory(test)



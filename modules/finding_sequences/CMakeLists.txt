
cmake_minimum_required(VERSION 3.11-3.18)

project(
    finding_sequences
    VERSION 1
    LANGUAGES CXX
)

include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup()



add_subdirectory(src)
if(CMAKE_PROJECT_NAME STREQUAL PROJECT_NAME AND BUILD_TESTING)
    add_subdirectory(test)
endif()



enable_testing()
add_test(NAME full_test COMMAND full_test)
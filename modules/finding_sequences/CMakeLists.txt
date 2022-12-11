
cmake_minimum_required(VERSION 3.18)
#set(CONAN_DISABLE_CHECK_COMPILER True CACHE BOOL "" FORCE )

project(
    finding_sequences
    VERSION 1
    LANGUAGES CXX
)
set(CMAKE_CXX_STANDARD 20)


add_subdirectory(library)
add_subdirectory(test)

enable_testing()

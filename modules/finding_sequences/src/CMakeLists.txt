set(AI_ROOT "C:/prj/ai/")
add_compile_definitions(FINDING_SEQUENCES_EXPORTS)

cmake_minimum_required(VERSION 3.11-3.18)

project(lib_finding_sequences)

include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup()


add_library(${PROJECT_NAME} SHARED
    src/Main/finding_sequences_interface.cpp
    src/Main/main.cpp
    src/Database/Database.cpp
    src/Computational_resource/Computational_resource.cpp

    src/Finding_sequences/Finding_sequences.cpp
    src/Finding_sequences/Interval_in_sequence/Interval_in_sequence.cpp
    src/Finding_sequences/Iteration_state_of_searching_pairs/Iteration_state_of_searching_pairs.cpp
)


target_link_libraries(${PROJECT_NAME} PRIVATE ${CONAN_LIBS})



target_include_directories(
    ${PROJECT_NAME} PUBLIC ${PROJECT_SOURCE_DIR}/include
)
target_include_directories(
    ${PROJECT_NAME} PRIVATE ${CONAN_INCLUDE_DIRS}
)

# basic data structures (Figure, Appearance, Edge etc.)
add_subdirectory(
    ${AI_ROOT}/cpp/basic_data_structures
    subproject/basic_data_structures
)
target_include_directories(${PROJECT_NAME} PRIVATE
    ${AI_ROOT}/cpp/basic_data_structures/src
)


set_property(TARGET ${PROJECT_NAME} PROPERTY CXX_STANDARD 20)


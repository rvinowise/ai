cmake_minimum_required(VERSION 3.18)

set(AI_ROOT "C:/prj/ai/")
add_compile_definitions(FINDING_SEQUENCES_EXPORTS)


include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup()


add_library(${PROJECT_NAME}_lib STATIC
    Main/finding_sequences_interface.cpp
    Main/main.cpp
    Database/Database.cpp
    Computational_resource/Computational_resource.cpp
    Finding_sequences/Finding_sequences.cpp
    Finding_sequences/Interval_in_sequence/Interval_in_sequence.cpp
    Finding_sequences/Iteration_state_of_searching_pairs/Iteration_state_of_searching_pairs.cpp
    
)
add_library(rvinowise::ai::finding_sequences ALIAS ${PROJECT_NAME}_lib )
target_link_libraries(${PROJECT_NAME}_lib PUBLIC rvinowise::ai::basic_data_structures)



target_link_libraries(${PROJECT_NAME}_lib PRIVATE ${CONAN_LIBS})
target_include_directories(
    ${PROJECT_NAME}_lib PUBLIC ${CONAN_INCLUDE_DIRS}
)



target_include_directories(
    ${PROJECT_NAME}_lib PUBLIC
    ${PROJECT_SOURCE_DIR}/include
    ${PROJECT_SOURCE_DIR}/src
)
target_include_directories(
    ${PROJECT_NAME}_lib PUBLIC
    "C:/prj/ai/cpp/basic_data_structures/src/"
)

# basic data structures (Figure, Appearance, Edge etc.)
add_subdirectory(
    ${AI_ROOT}/cpp/basic_data_structures
    subproject/basic_data_structures
)


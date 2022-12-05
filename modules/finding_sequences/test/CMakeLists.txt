set(AI_ROOT "C:/prj/ai/")

cmake_minimum_required(VERSION 3.18)


include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup()

add_executable(${PROJECT_NAME}_test
    catch2/finding_sequences.cpp
)

target_link_libraries(${PROJECT_NAME}_test PRIVATE ${CONAN_LIBS})
target_include_directories(
    ${PROJECT_NAME}_test PRIVATE ${CONAN_INCLUDE_DIRS}
)


add_library(finding_sequences_lib SHARED
    ../src/Main/finding_sequences_interface.cpp
    ../src/Main/main.cpp
    ../src/Database/Database.cpp
    ../src/Computational_resource/Computational_resource.cpp
    ../src/Finding_sequences/Finding_sequences.cpp
    ../src/Finding_sequences/Interval_in_sequence/Interval_in_sequence.cpp
    ../src/Finding_sequences/Iteration_state_of_searching_pairs/Iteration_state_of_searching_pairs.cpp
)

target_link_libraries(${PROJECT_NAME}_test
    PRIVATE 
    finding_sequences_lib
)



# target_include_directories(
#     ${PROJECT_NAME}_test PRIVATE 
#     ${PROJECT_SOURCE_DIR}/include
#     ${PROJECT_SOURCE_DIR}/src
# )



#add_test(NAME test COMMAND $<TARGET_FILE:finding_sequences_test>)
add_test(NAME test2 COMMAND finding_sequences_test)
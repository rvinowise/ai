set(AI_ROOT "C:/prj/ai/")

cmake_minimum_required(VERSION 3.11-3.18)

add_executable(${PROJECT_NAME}_test
    catch2/finding_sequences.cpp
)
target_link_libraries(${PROJECT_NAME}_test PUBLIC ${PROJECT_NAME}_lib)

target_include_directories(
    ${PROJECT_NAME}_test PUBLIC 
    ${PROJECT_SOURCE_DIR}/include
    ${PROJECT_SOURCE_DIR}/src
)

add_test(NAME ${PROJECT_NAME}_test COMMAND ${PROJECT_NAME}_test)
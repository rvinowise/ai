set(AI_ROOT "C:/prj/ai/")

cmake_minimum_required(VERSION 3.18)

# include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
# conan_basic_setup()

add_executable(${PROJECT_NAME}_test
    catch2/finding_sequences.cpp
)




target_link_libraries(${PROJECT_NAME}_test PRIVATE ${CONAN_LIBS})
target_include_directories(
    ${PROJECT_NAME}_test PRIVATE ${CONAN_INCLUDE_DIRS}
)

target_link_libraries(${PROJECT_NAME}_test
    PUBLIC 
    rvinowise::ai::finding_sequences
)
# target_include_directories(
#     ${PROJECT_NAME}_test PRIVATE 
#     ${PROJECT_SOURCE_DIR}/include
#     ${PROJECT_SOURCE_DIR}/src
# )

# add_test(NAME ${PROJECT_NAME}_test COMMAND ${PROJECT_NAME}_test)
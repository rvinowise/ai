set(AI_ROOT "C:/prj/ai/")
add_compile_definitions(finding_sequences_EXPORTS)

cmake_minimum_required(VERSION 3.11-3.18)

project(finding_sequences)

add_library(${PROJECT_NAME} SHARED src/finding_sequences.cpp)

# PUBLIC needed to make both hello.h and hello library available elsewhere in project
target_include_directories(
    ${PROJECT_NAME} PUBLIC ${PROJECT_SOURCE_DIR}/include
)

add_subdirectory(
    ${AI_ROOT}/cpp/basic_data_structures/ 
    subproject/basic_data_structures
)
# target_link_libraries(${PROJECT_NAME} PUBLIC basic_data_structures)
# target_include_directories(${PROJECT_NAME} PUBLIC
#     "${AI_ROOT}/cpp/basic_data_structures/src"
# )

target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)
set(AI_ROOT "C:/prj/ai/")
add_compile_definitions(FINDING_SEQUENCES_EXPORTS)

cmake_minimum_required(VERSION 3.11-3.18)

project(finding_sequences)

include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup()

add_library(${PROJECT_NAME} SHARED src/finding_sequences.cpp)


target_link_libraries(${PROJECT_NAME} ${CONAN_LIBS})



target_include_directories(
    ${PROJECT_NAME} PUBLIC ${PROJECT_SOURCE_DIR}/include
)
target_include_directories(
    ${PROJECT_NAME} PUBLIC ${CONAN_INCLUDE_DIRS}
)

add_subdirectory(
    ${AI_ROOT}/cpp/basic_data_structures/ 
    subproject/basic_data_structures
)



#target_link_libraries(main PRIVATE SOCI::soci_core SOCI::soci_empty)

# target_link_libraries(${PROJECT_NAME} PUBLIC basic_data_structures)
# target_include_directories(${PROJECT_NAME} PUBLIC
#     "${AI_ROOT}/cpp/basic_data_structures/src"
# )



target_compile_features(${PROJECT_NAME} PUBLIC cxx_std_20)



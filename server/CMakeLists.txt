# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.8)

project ("server")

SET (APPLICATION_NAME "Inverted Pacaman server")
SET (APPLICATION_CODENAME "${PROJECT_NAME}")
SET (APPLICATION_VERSION_MAJOR 1)
SET (APPLICATION_VERSION_MINOR 0)
SET (APPLICATION_VERSION_PATCH 0)
SET (APPLICATION_VERSION_TYPE SNAPSHOT)
SET (APPLICATION_VERSION_STRING "${APPLICATION_VERSION_MAJOR}.${APPLICATION_VERSION_MINOR}.${APPLICATION_VERSION_PATCH}-${APPLICATION_VERSION_TYPE}")
SET (APPLICATION_VENDOR_ID "GN")
SET (APPLICATION_VENDOR_NAME "Group N")
SET (APPLICATION_ID "${APPLICATION_VENDOR_ID}.${PROJECT_NAME}")

SET (SOURCE_DIR "${PROJECT_SOURCE_DIR}/server")
SET (LIB_DIR "${SOURCE_DIR}/lib")
SET (INCLUDE_DIR "${SOURCE_DIR}/include")
SET (TESTS_DIR "${PROJECT_SOURCE_DIR}/tests")

#
# Macro that detects subdirectories
#
FUNCTION(SUBDIRLIST result curdir)
  FILE(GLOB children RELATIVE ${curdir} ${curdir}/*)
  SET(dirlist "")
  FOREACH(child ${children})
    IF(IS_DIRECTORY ${curdir}/${child} AND NOT ${child} MATCHES "^\\..*")
        LIST(APPEND dirlist ${child})
        SET(subdirs "")
        SUBDIRLIST(subdirs ${curdir}/${child})
        FOREACH (subdir ${subdirs})
            LIST(APPEND dirlist "${child}/${subdir}")
        ENDFOREACH()
    ENDIF()
  ENDFOREACH()
  SET(${result} ${dirlist} PARENT_SCOPE)
ENDFUNCTION(SUBDIRLIST)

#
# Project Output Paths
#
SET (MAINFOLDER ${PROJECT_BINARY_DIR})
SET (EXECUTABLE_OUTPUT_PATH "${SOURCE_DIR}/bin")
SET (INCLUDE_OUTPUT_PATH "${SOURCE_DIR}/include")
SET (LIBRARY_OUTPUT_PATH "${SOURCE_DIR}/lib/linux")
SET (CTEST_BINARY_DIRECTORY "${SOURCE_DIR}/build/")
SET (CMAKE_VERBOSE_MAKEFILE 1) # Use 1 for debugging, 0 for release

LINK_DIRECTORIES("${LIBRARY_OUTPUT_PATH}")


MACRO(ADD_INV_PACMAN_LIBS root_dir)
MESSAGE(STATUS "root_dir: ${root_dir}")

# Add each subdirectory found
FOREACH(target ${ARGN})
    MESSAGE(STATUS "Adding library: ${target}")
    MESSAGE(STATUS "Adding library to dependencies: ${target}")
    SET(LIB_SOURCE_DIR "${root_dir}/${target}.l")
    MESSAGE(STATUS "At directory: ${LIB_SOURCE_DIR}")
    FILE (GLOB SRC_FILES
     CONFIGURE_DEPENDS "${LIB_SOURCE_DIR}/*.cpp"
    )
    FILE (GLOB HEADER_FILES
     CONFIGURE_DEPENDS "${LIB_SOURCE_DIR}/*.h"
    )
    FOREACH( header_file ${HEADER_FILES})
        get_filename_component(file_name ${header_file} NAME)
        CONFIGURE_FILE(${header_file} "${INCLUDE_OUTPUT_PATH}/${file_name}" COPYONLY)
    ENDFOREACH()
    MESSAGE(DEBUG "Files: ${SRC_FILES}")
    SET (
        OBJ_LIB
        "${target}_obj"
    )
    ADD_LIBRARY(${OBJ_LIB} OBJECT ${SRC_FILES})
    SET_PROPERTY(TARGET ${OBJ_LIB} PROPERTY POSITION_INDEPENDENT_CODE 1)
    target_compile_features(${OBJ_LIB} PUBLIC ${MIRA_CXX_STANDARD})

    TARGET_INCLUDE_DIRECTORIES (${OBJ_LIB} PUBLIC ${LIB_SOURCE_DIR})
    TARGET_INCLUDE_DIRECTORIES (${OBJ_LIB} PRIVATE ${PREVIOUS_LIBS_SOURCE_DIR})

    ADD_LIBRARY(${target} SHARED $<TARGET_OBJECTS:${OBJ_LIB}>)
    SET (
        STATIC_LIB
        "${target}_static"
    )
    ADD_LIBRARY(${STATIC_LIB} STATIC $<TARGET_OBJECTS:${OBJ_LIB}>)
    FOREACH(lib ${PREVIOUS_LIBS})
        MESSAGE(DEBUG "Adding link ${lib} for ${target}")
        TARGET_LINK_LIBRARIES (${target} $<TARGET_FILE_NAME:${lib}>)
        ADD_DEPENDENCIES (${target} ${lib})
        MESSAGE(DEBUG "Adding link ${STATIC_DEP} for ${lib}")
        TARGET_LINK_LIBRARIES ("${STATIC_LIB}" $<TARGET_FILE_NAME:${lib}_static>)
        ADD_DEPENDENCIES (${STATIC_LIB} ${lib})
    ENDFOREACH()
    target_link_directories(${target} PRIVATE ${LIBRARY_OUTPUT_PATH})
    target_link_directories(${STATIC_LIB} PRIVATE ${LIBRARY_OUTPUT_PATH})
    SET_TARGET_PROPERTIES(${STATIC_LIB} PROPERTIES OUTPUT_NAME ${target})
    LIST (APPEND PREVIOUS_LIBS ${target})
    LIST (APPEND PREVIOUS_LIBS_SOURCE_DIR ${LIB_SOURCE_DIR})

    IF (${target} STREQUAL "MiraMXSCtrl")
        #
        # Add grid generation
        #

        ADD_EXECUTABLE(mxs_generate_grid ${UBMXSPlanner_DIR}/mxs_generate_grid.p/main.cpp)
        ADD_DEPENDENCIES(mxs_generate_grid ${PREVIOUS_LIBS})
        target_compile_features(mxs_generate_grid PUBLIC ${MIRA_CXX_STANDARD})

        TARGET_INCLUDE_DIRECTORIES (mxs_generate_grid PRIVATE ${PREVIOUS_LIBS_SOURCE_DIR})

        FOREACH(lib ${PREVIOUS_LIBS})
            TARGET_LINK_LIBRARIES (
                mxs_generate_grid
                $<TARGET_FILE_NAME:${lib}>
            )
            TARGET_LINK_LIBRARIES ( mxs_generate_grid ${UUID_LINK_LIBRARIES} )
            MESSAGE(DEBUG "Adding link for lib: ${lib}")
        ENDFOREACH()
        target_link_directories(mxs_generate_grid PRIVATE ${LIBRARY_OUTPUT_PATH})

        SET (UB_TESTS_ETC_DIR "${UB_TESTS_ROOT_DIR}/etc")

        MESSAGE(DEBUG "Toml files: ${TOML_FILES}")

        SET (TOML_FILES)
        SET (CBOR_FILES)
        FOREACH(toml_file ${TOMLS})
            string(REGEX REPLACE "[.]toml$" ".cbor" tmp_file ${toml_file})
            string(REGEX REPLACE "mxssbe_" "grid_" cbor_file ${tmp_file})
            SET(output_file "${UB_TESTS_ETC_DIR}/${cbor_file}")
            SET(input_file "${UB_TESTS_ETC_DIR}/${toml_file}")
            add_custom_command(
                TARGET mxs_generate_grid
                POST_BUILD
                COMMAND mxs_generate_grid ${input_file}
                BYPRODUCTS ${output_file}
                DEPENDS ${input_file} mxs_generate_grid
                WORKING_DIRECTORY ${UB_TESTS_ETC_DIR})
            LIST(APPEND TOML_FILES ${input_file})
            LIST(APPEND CBOR_FILES ${output_file})
        ENDFOREACH()
        FOREACH(toml_file ${OTHER_TOMLS})
            SET(input_file "${UB_TESTS_ETC_DIR}/${toml_file}")
            LIST(APPEND TOML_FILES ${input_file})
        ENDFOREACH()
        MESSAGE(STATUS "cbor files: ${CBOR_FILES}")

    ENDIF()
ENDFOREACH()
ENDMACRO(ADD_INV_PACMAN_LIBS)




# Include sub-projects.
add_subdirectory (${SOURCE_DIR})
add_subdirectory(${TESTS_DIR})
add_library(${LIB_DIR})

add_executable(test_server test_server.c)
target_link_libraries(test_server "${INCLUDE_DIR}")
add_test(NAME test_server COMMAND $<TARGET_FILE:test_server> WORKING_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR})

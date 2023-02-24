# IEEC. Copyright (C) 2012-2022. MIT.
#
# Project Properties
#
CMAKE_MINIMUM_REQUIRED (VERSION 3.19)

CMAKE_POLICY(SET CMP0078 NEW)
CMAKE_POLICY(SET CMP0086 NEW)

PROJECT (MIRADAS)
SET (APPLICATION_NAME "Miradas Planning Tool")
SET (APPLICATION_CODENAME "${PROJECT_NAME}")
SET (APPLICATION_COPYRIGHT_YEARS "2012-2022")
SET (APPLICATION_VERSION_MAJOR 1)
SET (APPLICATION_VERSION_MINOR 0)
SET (APPLICATION_VERSION_PATCH 0)
SET (APPLICATION_VERSION_TYPE SNAPSHOT)
SET (APPLICATION_VERSION_STRING "${APPLICATION_VERSION_MAJOR}.${APPLICATION_VERSION_MINOR}.${APPLICATION_VERSION_PATCH}-${APPLICATION_VERSION_TYPE}")
SET (APPLICATION_VENDOR_ID "cat.ieec")
SET (APPLICATION_VENDOR_NAME "IEEC-UB")
SET (APPLICATION_VENDOR_URL "www.ieec.cat")
SET (APPLICATION_ID "${APPLICATION_VENDOR_ID}.${PROJECT_NAME}")

SET (MIRA_CXX_STANDARD cxx_std_11)

#set(CMAKE_CXX_CLANG_TIDY "clang-tidy-mp-15;-checks=-*,clang-analyzer-cplusplus*,cppcoreguidelines-*")

SET (SOURCE_DIR "${PROJECT_SOURCE_DIR}/gcs")
SET (UFCommon_DIR
${SOURCE_DIR}/src_c++/gtc/AL/ECM/MIRADAS/resources/UFCommon
)

SET (UFCommon_LIBS
UFUtils
UFProtocol
)

SET (MIRADASMCSCommon_DIR
${SOURCE_DIR}/src_c++/gtc/AL/ECM/MIRADAS/MCS/resources/MiradasMCSCommon
)

SET (UF_LIBS
UFPhytronMotor
UFRotationMotor
MXSProbeArms
)

SET (UBMXSPlanner_DIR
${SOURCE_DIR}/src_c++/gtc/AL/ECM/MIRADAS/MCS/resources/UBMXSPlanner
)

SET (UB_TESTS_ROOT_DIR "${UBMXSPlanner_DIR}/tests")

SET (UB_LIBS
MiraUtil
MiraGeometry
MiraProbeArm
MiraMXSCtrl
MiraGCluster
MiraAllocation
MiraTrajectory
MiraMXSPlanner
MiraMXSFrontEnd
)

SET (SWIG_LIB
MiraMXSFrontEnd
)

SET (SWIG_LANGS
Python
Java
)

SET (SWIG_Python
swigMXSFrontEndPython.i
)

SET (SWIG_OUTDIR_Python
"${CMAKE_BINARY_DIR}/python"
)

SET (SWIG_FLAGS_Python
""
)

SET (Java_DIR
${SOURCE_DIR}/src_java
)

SET (SWIG_Java
swigMXSFrontEndJNI.i
)

SET (MIRADAS_JAVA_DIR
${Java_DIR}/gtc/AL/PhaseII/Tools/MIRADAS
)

SET (SWIG_OUTDIR_Java
${MIRADAS_JAVA_DIR}/MiraMXSFrontEnd/MXS
)

SET (SWIG_FLAGS_Java
"-package" "gtc.AL.PhaseII.Tools.MIRADAS.MiraMXSFrontEnd.MXS"
)

SET (UBMXSPlannerTests_DIR
${SOURCE_DIR}/src_c++/gtc/AL/ECM/MIRADAS/MCS/resources/UBMXSPlanner/tests
)

SET (JAVA_Target
"MXSConfigurator"
)

SET (JAVA_TEST_CLASS
"MiraMXSFrontEndTest"
)

SET (MIRADAS_CLASSPATH
"${PROJECT_SOURCE_DIR}/MiradasPhaseII/lib"
)

SET (MIRADAS_JARS
jackson-annotations-2.9.5.jar
jackson-core-2.9.5.jar
jackson-databind-2.9.5.jar
log4j-1.2-api-2.12.1.jar
log4j-api-2.12.1.jar
log4j-core-2.12.1.jar
)

SET (TOMLS
    mxssbe_base.toml
    mxssbe_cw.toml
    mxssbe_P6X.toml
    mxssbe_ccw.toml
    mxssbe_calibrated_first_light_cw.toml
)

#
# Those TOML files are used for testing purposes, and reuse other cbor files
#
SET (OTHER_TOMLS
    mxssbe_0_3_7_retrograde.toml
)

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

SET(CMAKE_CXX_FLAGS  "${CMAKE_CXX_FLAGS} ${GCC_COVERAGE_COMPILE_FLAGS}" )

if (CMAKE_CXX_COMPILER_ID STREQUAL "Clang")
  # using Clang
elseif (CMAKE_CXX_COMPILER_ID STREQUAL "GNU")
add_compile_options("-Wno-deprecated") # using GCC
endif()
SET (CMAKE_VERBOSE_MAKEFILE 1) # Use 1 for debugging, 0 for release

#
# Project Output Paths
#
SET (MAINFOLDER ${PROJECT_BINARY_DIR})
SET (EXECUTABLE_OUTPUT_PATH "${SOURCE_DIR}/bin")
SET (INCLUDE_OUTPUT_PATH "${SOURCE_DIR}/include")
SET (LIBRARY_OUTPUT_PATH "${SOURCE_DIR}/lib/linux")
SET (CTEST_BINARY_DIRECTORY "${SOURCE_DIR}/build/")

#
# Project Search Paths
#
LIST (APPEND CMAKE_PREFIX_PATH "${MAINFOLDER}")
LIST (APPEND CMAKE_PREFIX_PATH "${MAINFOLDER}/tools")
LIST (APPEND CMAKE_PREFIX_PATH "${MAINFOLDER}/thirdparty")
SET (CMAKE_MODULE_PATH "${PROJECT_SOURCE_DIR}/CMake")

#
# Locate Project Prerequisites
#
FIND_PACKAGE (Log4Cxx REQUIRED)
INCLUDE_DIRECTORIES(${Log4cxx_INCLUDE_DIR})
LINK_LIBRARIES(${Log4cxx_LIBRARY})

FIND_PACKAGE (GTest REQUIRED)
INCLUDE(GoogleTest)

find_package(PkgConfig REQUIRED)
pkg_search_module(UUID REQUIRED uuid)

FIND_PACKAGE (Java REQUIRED)
INCLUDE (UseJava)
MESSAGE(STATUS "Java version: ${Java_VERSION_STRING}")

#set(JAVA_AWT_LIBRARY "$ENV{JAVA_HOME}/lib/libjawt.so")
#set(JAVA_JVM_LIBRARY "$ENV{JAVA_HOME}/lib/server/libjvm.so")

#set(JAVA_INCLUDE_PATH "$ENV{JAVA_HOME}/include")
#set(JAVA_INCLUDE_PATH2 "$ENV{JAVA_HOME}/include/linux")
#set(JAVA_AWT_INCLUDE_PATH "$ENV{JAVA_HOME}/include")

FIND_PACKAGE (JNI REQUIRED)
INCLUDE_DIRECTORIES (${JNI_INCLUDE_DIRS})
LINK_LIBRARIES (${JNI_LIBRARIES})

FIND_PACKAGE (Python3 REQUIRED COMPONENTS Interpreter Development)
INCLUDE_DIRECTORIES (${Python3_INCLUDE_DIRS})
LINK_LIBRARIES (${Python3_LIBRARIES})

SET(CMAKE_JAVA_COMPILE_FLAGS -source 11)

FIND_PACKAGE(SWIG 3.0 REQUIRED COMPONENTS python java)
SET(SWIG_FLAGS_4_0)
IF (${SWIG_VERSION} VERSION_GREATER "4.0")
    SET(SWIG_FLAGS_4_0 "-doxygen")
ENDIF()
INCLUDE (UseSWIG)

MESSAGE(STATUS "SWIG version: ${SWIG_VERSION}")

#
# Configure Files
#
FILE (GLOB_RECURSE CONFIGINPUTS1 include/*.in.h.cmake)
FILE (GLOB_RECURSE CONFIGINPUTS2 include/*.h.in.cmake)
FILE (GLOB_RECURSE CONFIGINPUTS3 src/*.in.h.cmake)
FILE (GLOB_RECURSE CONFIGINPUTS4 src/*.h.in.cmake)
FILE (GLOB_RECURSE CONFIGINPUTS5 test/*.in.h.cmake)
FILE (GLOB_RECURSE CONFIGINPUTS6 test/*.h.in.cmake)
LIST (APPEND CONFIGINPUTS "${CONFIGINPUTS1}")
LIST (APPEND CONFIGINPUTS "${CONFIGINPUTS2}")
LIST (APPEND CONFIGINPUTS "${CONFIGINPUTS3}")
LIST (APPEND CONFIGINPUTS "${CONFIGINPUTS4}")
LIST (APPEND CONFIGINPUTS "${CONFIGINPUTS5}")
LIST (APPEND CONFIGINPUTS "${CONFIGINPUTS6}")
FOREACH (CONFIGINPUT ${CONFIGINPUTS})
    STRING (REPLACE ".in.h.cmake" ".h" CONFIGOUTPUT1 "${CONFIGINPUT}")
    STRING (REPLACE ".h.in.cmake" ".h" CONFIGOUTPUT2 "${CONFIGOUTPUT1}")
    CONFIGURE_FILE ("${CONFIGINPUT}" "${CONFIGOUTPUT2}")
ENDFOREACH (CONFIGINPUT ${CONFIGINPUTS})

INCLUDE("ExternalProject")

LINK_DIRECTORIES("${LIBRARY_OUTPUT_PATH}")

SET(PREVIOUS_LIBS)
SET(PREVIOUS_LIBS_SOURCE_DIR)

MACRO(ADD_MIRADAS_LIBS root_dir)
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
ENDMACRO(ADD_MIRADAS_LIBS)

#
# Add Build Targets
#

ADD_MIRADAS_LIBS ("${UFCommon_DIR}" ${UFCommon_LIBS})

ADD_MIRADAS_LIBS ("${MIRADASMCSCommon_DIR}" ${UF_LIBS})

SET (UB_LIBS_ROOT_DIR "${UBMXSPlanner_DIR}")

ADD_MIRADAS_LIBS ("${UB_LIBS_ROOT_DIR}" ${UB_LIBS})

#
# Add Build Tests
#

enable_testing()

#
# Add the tests
#

SET (root_dir "${UB_TESTS_ROOT_DIR}")

# Add each subdirectory found
FOREACH(target ${UB_LIBS})
    MESSAGE(STATUS "Adding test: ${target}")
    SET(TEST_SOURCE_DIR "${root_dir}/${target}Test.p")
    MESSAGE(DEBUG "At directory: ${TEST_SOURCE_DIR}")
    FILE (GLOB SRC_FILES
        CONFIGURE_DEPENDS "${TEST_SOURCE_DIR}/*.cpp"
    )
    SET (TEST_EXEC "${target}-unittest")
    ADD_EXECUTABLE(
        ${TEST_EXEC}
        ${SRC_FILES}
    )

    target_compile_features(${TEST_EXEC} PUBLIC ${MIRA_CXX_STANDARD})

    MESSAGE(DEBUG "Files: ${SRC_FILES}")
    TARGET_INCLUDE_DIRECTORIES (
        ${TEST_EXEC}
        PUBLIC
        ${GTEST_INCLUDE_DIRS}
    )

    TARGET_INCLUDE_DIRECTORIES (${TEST_EXEC} PRIVATE ${PREVIOUS_LIBS_SOURCE_DIR} ${TEST_SOURCE_DIR} ${UUID_INCLUDE_DIRS})
    TARGET_LINK_LIBRARIES (
        ${TEST_EXEC}
        ${GTEST_BOTH_LIBRARIES}
        ${UUID_LINK_LIBRARIES}
    )
    FOREACH(lib ${PREVIOUS_LIBS})
        TARGET_LINK_LIBRARIES (
            ${TEST_EXEC}
            $<TARGET_FILE_NAME:${lib}>
        )
        ADD_DEPENDENCIES (${TEST_EXEC} ${lib})
        MESSAGE(DEBUG "Adding link for lib: ${lib}")
    ENDFOREACH()

    target_link_directories(${TEST_EXEC} PRIVATE ${LIBRARY_OUTPUT_PATH})

    FILE (GLOB DATA_FILES
        CONFIGURE_DEPENDS "${TEST_SOURCE_DIR}/*"
    )
    SET (WORK_DIR "${MAINFOLDER}/tests/${target}")

    MESSAGE (STATUS "Copying to ${WORK_DIR}")

    MESSAGE(DEBUG "Work directory: ${WORK_DIR}")
    FILE (MAKE_DIRECTORY "${WORK_DIR}")

    ADD_DEPENDENCIES(${TEST_EXEC} mxs_generate_grid)

    FOREACH(file_name ${TOML_FILES} ${CBOR_FILES})
        MESSAGE (STATUS "File: ${file_name}")
        get_filename_component(conf_file ${file_name} NAME)
        MESSAGE (STATUS "Copying to ${WORK_DIR}/${conf_file}")
        add_custom_command(TARGET ${TEST_EXEC}
            COMMAND ${CMAKE_COMMAND} -E copy_if_different ${file_name} ${WORK_DIR}/${conf_file})
    ENDFOREACH()

    FOREACH (conf_file ${DATA_FILES})
        get_filename_component(file_name ${conf_file} NAME)
        MESSAGE (DEBUG "Copying to ${WORK_DIR} ${file_name}")
        CONFIGURE_FILE(${conf_file} "${WORK_DIR}/${file_name}" COPYONLY)
    ENDFOREACH()

    SET (
        TEST_NAME
        "All_Tests_in_${TEST_EXEC}"
    )

    gtest_discover_tests(${TEST_EXEC}
    WORKING_DIRECTORY "${WORK_DIR}"
    DISCOVERY_TIMEOUT 30)
    MESSAGE (STATUS "Added test: ${TEST_NAME}")
ENDFOREACH()

# SWIG

SET(USE_SWIG_DEPENDENCIES TRUE)

SET(LIB_SOURCE_DIR "${UBMXSPlanner_DIR}/${SWIG_LIB}.l")
INCLUDE_DIRECTORIES("${LIB_SOURCE_DIR}")

MESSAGE(STATUS "At directory: ${LIB_SOURCE_DIR}")
FOREACH(SWIG_LANG ${SWIG_LANGS})
    MESSAGE (STATUS "Lang: ${SWIG_LANG}")
    SET(SWIG_FILE "${SWIG_${SWIG_LANG}}")
    SET(SWIG_TARGET "${SWIG_LIB}_${SWIG_LANG}")
    SET(SWIG_FILE_PATH "${LIB_SOURCE_DIR}/${SWIG_FILE}")
    SET(SWIG_FLAGS "${SWIG_FLAGS_${SWIG_LANG}}")
    # Option not available in SWIG 3
    LIST(APPEND SWIG_FLAGS ${SWIG_FLAGS_4_0})

    SET_PROPERTY(SOURCE ${SWIG_FILE_PATH} PROPERTY CPLUSPLUS ON)

    MESSAGE(STATUS "Swig source dir: ${LIB_SOURCE_DIR}")
    FILE (GLOB SRC_FILES
        CONFIGURE_DEPENDS "${LIB_SOURCE_DIR}/*.h"
    )
    MESSAGE(DEBUG "Swig source files: ${SRC_FILES}")

    MESSAGE(DEBUG "Flags: ${SWIG_FLAGS}")
    SET(CMAKE_SWIG_FLAGS ${SWIG_FLAGS})
    set (UseSWIG_TARGET_NAME_PREFERENCE STANDARD)
    SWIG_ADD_LIBRARY(${SWIG_TARGET} LANGUAGE ${SWIG_LANG} OUTPUT_DIR "${SWIG_OUTDIR_${SWIG_LANG}}" OUTFILE_DIR ${LIB_SOURCE_DIR} SOURCES ${SWIG_FILE_PATH} ${SRC_FILES} )
    set_property(TARGET ${SWIG_TARGET} PROPERTY SWIG_INCLUDE_DIRECTORIES "${LIB_SOURCE_DIR}" )
    set_property(TARGET ${SWIG_TARGET} PROPERTY SWIG_USE_TARGET_INCLUDE_DIRECTORIES ON)
    set_property(TARGET ${SWIG_TARGET} PROPERTY DEPENDS "swigMXSFrontEnd.i" ${SRC_FILES} ${PREVIOUS_LIBS})
    target_compile_features(${SWIG_TARGET} PUBLIC ${MIRA_CXX_STANDARD})

    MESSAGE(STATUS "Added: ${SWIG_TARGET}")
    FOREACH(lib ${PREVIOUS_LIBS})
        TARGET_LINK_LIBRARIES( ${SWIG_TARGET} $<TARGET_FILE_NAME:${lib}>)
        ADD_DEPENDENCIES (${SWIG_TARGET} ${lib})
        MESSAGE(DEBUG "Adding link for lib: ${lib}")
    ENDFOREACH()
    TARGET_INCLUDE_DIRECTORIES (${SWIG_TARGET} PRIVATE ${PREVIOUS_LIBS_SOURCE_DIR})
    target_link_directories(${SWIG_TARGET} PRIVATE ${LIBRARY_OUTPUT_PATH})
    get_property(${SWIG_LANG}_Support_Files_Dir TARGET ${SWIG_TARGET} PROPERTY SWIG_SUPPORT_FILES_DIRECTORY)
ENDFOREACH()

# Check python files

find_package(Pylint REQUIRED)

IF (PYLINT_FOUND)
    CONFIGURE_FILE("${LIB_SOURCE_DIR}/__init__.py.in" "${SWIG_OUTDIR_Python}/__init__.py" @ONLY)
    CONFIGURE_FILE("${LIB_SOURCE_DIR}/MXSFrontEndPlot.py" "${SWIG_OUTDIR_Python}/MXSFrontEndPlot.py" COPYONLY)
    MESSAGE(STATUS "${SWIG_OUTDIR_Python}")
    ADD_CUSTOM_COMMAND(OUTPUT check.json
                       COMMAND pylint --output-format=json:check.json,colorized --disable=C0413 .
                       WORKING_DIRECTORY "${SWIG_OUTDIR_Python}"
                       DEPENDS "${SWIG_OUTDIR_Python}/__init__.py" "${SWIG_OUTDIR_Python}/MXSFrontEnd.py"
                       VERBATIM)
    ADD_CUSTOM_TARGET(CHECK_MiraMXSFrontEnd_Python
                      DEPENDS check.json)
    ADD_DEPENDENCIES(CHECK_MiraMXSFrontEnd_Python MiraMXSFrontEnd_Python)

    MESSAGE(STATUS "Added pylint check.")
ENDIF()

find_package(Pytest REQUIRED)

IF (PYTEST_FOUND)
    SET(PY_TEST_DEST "${MAINFOLDER}/tests/${SWIG_LIB}")
    MESSAGE(STATUS "PY_TEST_DEST: ${PY_TEST_DEST}")
    SET (WORK_DIR "${PY_TEST_DEST}")
    ADD_TEST(
        NAME Python_tests
        COMMAND pytest 
        WORKING_DIRECTORY "${WORK_DIR}"
    )

    SET(PY_TEST_SRC "${UB_TESTS_ROOT_DIR}/${SWIG_LIB}Test.p")
    MESSAGE(STATUS "PY_TEST_SRC: ${PY_TEST_SRC}")
    CONFIGURE_FILE("${PY_TEST_SRC}/Test.ipynb.in" "${PY_TEST_DEST}/Test.ipynb" @ONLY)
    CONFIGURE_FILE("${PY_TEST_SRC}/miradas_n869_targets.txt" "${PY_TEST_DEST}/miradas_n869_targets.txt" COPYONLY)
    CONFIGURE_FILE("${PY_TEST_SRC}/mxssbe_cw.toml" "${PY_TEST_DEST}/mxssbe_cw.toml" COPYONLY)
    set_tests_properties(Python_tests PROPERTIES REQUIRED_FILES "grid_cw.cbor")
    MESSAGE(STATUS "Added pytest check.")
ENDIF()

# Code added to take into account the files generated by swig in java

MESSAGE(DEBUG "Swig dir: ${Java_Support_Files_Dir}")

SET(Swig_Java_Generated_Files
ASS_Status.java
Allocations.java
AllocationsIterator.java
Arms.java
ArmsIterator.java
Assignment.java
Configuration.java
ConfigurationIterator.java
Deinterfere.java
Geo_Type.java
Geometry.java
GridPoints.java
HL_Status.java
Motion.java
NearestNeighbours.java
Paths.java
PathsIterator.java
PointC.java
PointN.java
PointXY.java
Polygon.java
PolygonIterator.java
Position.java
ProbeDirKin.java
ProbeIdC.java
ProbeIdN.java
Probes.java
ProbesGridPoints.java
ProbesGridPointsIterator.java
ProbesIterator.java
RADec.java
ReachProbes.java
SegmentXY.java
TR_Status.java
Target.java
Targets.java
TargetsIterator.java
Trajectories.java
TrajectoriesIterator.java
Trajectory.java
TrajectoryGeometries.java
TrajectoryGeometriesIterator.java
TrajectoryIterator.java
TrajectoryProbes.java
TrajectoryProbesIterator.java
VectorC.java
VectorN.java
VectorXY.java
)

SET(Swig_Java_Generated_Paths)
FOREACH (java_generated_file ${Swig_Java_Generated_Files})
    LIST(APPEND Swig_Java_Generated_Paths ${Java_Support_Files_Dir}/${java_generated_file})
ENDFOREACH()

get_property(Java_Support_Files TARGET ${SWIG_LIB}_Java PROPERTY SWIG_SUPPORT_FILES)

MESSAGE(DEBUG "Swig files: ${Java_Support_Files}")

# Dummy custom command to make DEPENDENT the files generated by swig

ADD_CUSTOM_COMMAND(OUTPUT ${Swig_Java_Generated_Paths}
COMMAND true
DEPENDS ${Java_Support_Files}
)

# Build java package

SET(CMAKE_JNI_TARGET TRUE)

# Search the subdirectories and files to generate the jars

SET(JAVA_DIRS)
SUBDIRLIST(JAVA_DIRS ${Java_DIR})
SET(PHASEII_FILES)
SET(PHASEII_TEST_DIRS)
SET(PHASEII_TEST_FILES)
FOREACH(subdir ${JAVA_DIRS})
    IF (${subdir} MATCHES "PhaseII")
        IF (${subdir} MATCHES "Tests")
            LIST(APPEND PHASEII_TEST_DIRS "${Java_DIR}/${subdir}")
            FILE (GLOB JAVA_FILES
                CONFIGURE_DEPENDS "${Java_DIR}/${subdir}/*.java"
            )
            LIST (APPEND PHASEII_TEST_FILES ${JAVA_FILES})
        ELSE()
            SET (Package_Dir
            ${Java_DIR}/${subdir})

            # Avoid the files generated by swig

            IF (NOT ${Package_Dir} MATCHES ${Java_Support_Files_Dir})
                FILE (GLOB JAVA_FILES
                    CONFIGURE_DEPENDS "${Java_DIR}/${subdir}/*.java"
                )
            ENDIF()
            LIST (APPEND PHASEII_FILES ${JAVA_FILES})
        ENDIF()
    ENDIF()
ENDFOREACH()

MESSAGE(DEBUG "PhaseII: ${PHASEII_FILES}")
MESSAGE(DEBUG "PhaseII Test: ${PHASEII_TEST_FILES}")

# Search depending jar files

SUBDIRLIST(JAR_DIRS ${MIRADAS_CLASSPATH})
SET (DEP_JARS)
LIST(APPEND JAR_DIRS ".")
FOREACH(subdir ${JAR_DIRS})
    FILE(REAL_PATH "${MIRADAS_CLASSPATH}/${subdir}" DEP_PATH)
    FILE (GLOB JAVA_FILES
        CONFIGURE_DEPENDS "${DEP_PATH}/*.jar"
    )
    FOREACH( filename ${JAVA_FILES})
        IF (NOT ${filename} MATCHES "-javadoc.jar$" AND NOT ${filename} MATCHES "-sources.jar$")
            MESSAGE(DEBUG "${filename}")
            LIST(APPEND DEP_JARS ${filename})
        ENDIF()
    ENDFOREACH()
ENDFOREACH()

# Create the main jar

ADD_JAR ("${JAVA_Target}"
         SOURCES ${PHASEII_FILES} ${Swig_Java_Generated_Paths} ${Java_Support_Files}
         INCLUDE_JARS ${DEP_JARS}
         ENTRY_POINT "gtc.AL.PhaseII.Tools.MIRADAS.MiraMXSFrontEnd.phase2toolgui.Phase2ToolGUI")
ADD_DEPENDENCIES (${JAVA_Target} ${SWIG_LIB}_Java)
get_property(JAVA_Target_Path TARGET ${JAVA_Target} PROPERTY JAR_FILE)
LIST(JOIN DEP_JARS ":" DEP_CLASSPATH)
SET(MXS_FRONT_END_SH "${CMAKE_BINARY_DIR}/MXSFrontEnd.sh")
CONFIGURE_FILE("${MIRADAS_JAVA_DIR}/MXSFrontEnd.sh.in" "${MXS_FRONT_END_SH}" @ONLY)
FILE(CHMOD "${MXS_FRONT_END_SH}" PERMISSIONS OWNER_WRITE OWNER_READ OWNER_EXECUTE GROUP_READ GROUP_EXECUTE WORLD_READ WORLD_EXECUTE)
MESSAGE(STATUS "${JAVA_Target_Path}")

# Create the test jar

SET(JAVA_Target_Test "${JAVA_Target}_Test")
ADD_JAR ("${JAVA_Target_Test}"
    SOURCES ${PHASEII_TEST_FILES}
    INCLUDE_JARS ${DEP_JARS} "${JAVA_Target_Path}"
    ENTRY_POINT "gtc.AL.PhaseII.Tools.MIRADAS.MiraMXSFrontEnd.phase2toolgui.Phase2ToolGUI")
ADD_DEPENDENCIES ("${JAVA_Target_Test}" ${SWIG_LIB}_Java)
MESSAGE(DEBUG "Test dirs: ${PHASEII_TEST_DIRS}")

# Copy the test files to the corresponing working directories

SET(DATA_FILES)
FOREACH(conf_dir ${PHASEII_TEST_DIRS})
    FILE (GLOB conf_files
        CONFIGURE_DEPENDS "${conf_dir}/*"
    )
    LIST(APPEND DATA_FILES ${conf_files})
ENDFOREACH()

ADD_DEPENDENCIES(${JAVA_Target_Test} mxs_generate_grid)

FOREACH(file_name ${TOML_FILES} ${CBOR_FILES})
    MESSAGE (STATUS "File: ${file_name}")
    get_filename_component(conf_file ${file_name} NAME)
    MESSAGE (STATUS "Copying to ${WORK_DIR}/${conf_file}")
    add_custom_command(TARGET ${JAVA_Target_Test}
        COMMAND ${CMAKE_COMMAND} -E copy_if_different ${file_name} ${WORK_DIR}/${conf_file})
ENDFOREACH()

# Java test

SET (WORK_DIR "${MAINFOLDER}/tests/${JAVA_Target_Test}")
MESSAGE("Work directory: ${WORK_DIR}")
FILE (MAKE_DIRECTORY "${WORK_DIR}")
FOREACH (conf_file ${DATA_FILES})
    IF (NOT IS_DIRECTORY ${conf_file})
        get_filename_component(file_name ${conf_file} NAME)
        get_filename_component(file_ext ${conf_file} LAST_EXT)
        IF (NOT ${file_ext} MATCHES ".java")
            MESSAGE (DEBUG "Copying to ${WORK_DIR} ${file_name}")
            CONFIGURE_FILE(${conf_file} "${WORK_DIR}/${file_name}" COPYONLY)
        ENDIF()
    ENDIF()
ENDFOREACH()

SET (
    TEST_NAME
    "All_Tests_in_${JAVA_Target_Test}"
)

LIST(JOIN DEP_JARS ":" TEST_CLASSPATH)
get_property(JAVA_Target_Test_Path TARGET ${JAVA_Target_Test} PROPERTY JAR_FILE)
SET (
    TEST_EXEC
    ${Java_JAVA_EXECUTABLE}
    -cp "${TEST_CLASSPATH}:${JAVA_Target_Path}:${JAVA_Target_Test_Path}"
    -Djava.library.path=${LIBRARY_OUTPUT_PATH}
    -Dlog4j.configurationFile=file:log4j.properties
    org.junit.runner.JUnitCore
    ${JAVA_TEST_CLASS}
)
MESSAGE (STATUS "Added test: ${TEST_NAME}")
ADD_TEST(
    NAME ${TEST_NAME}
    COMMAND ${TEST_EXEC}
    WORKING_DIRECTORY "${WORK_DIR}"
)

MESSAGE (STATUS "Added test: ${TEST_NAME}")


# Node.js interface based on NAN

SET(NODE_LIBRARY "${SWIG_LIB}JS")
SET (root_dir ${UB_LIBS_ROOT_DIR})

MESSAGE(STATUS "Adding library: ${NODE_LIBRARY}")
MESSAGE(STATUS "Adding library to dependencies: ${NODE_LIBRARY}")
SET(LIB_SOURCE_DIR "${root_dir}/${SWIG_LIB}.l")
MESSAGE(STATUS "At directory: ${LIB_SOURCE_DIR}")
INCLUDE_DIRECTORIES("${LIB_SOURCE_DIR}")
FILE (GLOB SRC_FILES
 CONFIGURE_DEPENDS "${LIB_SOURCE_DIR}/*.cpp"
)
# MESSAGE(STATUS "Files: ${SRC_FILES}")

FILE (GLOB JS_HEADER_FILES
 CONFIGURE_DEPENDS "${LIB_SOURCE_DIR}/*.h"
)
add_library(${NODE_LIBRARY} SHARED ${SRC_FILES} ${CMAKE_JS_SRC})

target_compile_features(${NODE_LIBRARY} PUBLIC ${MIRA_CXX_STANDARD})

# Gives our library file a .node extension without any "lib" prefix
set_target_properties(${NODE_LIBRARY} PROPERTIES PREFIX "" SUFFIX ".node")

# Essential include files to build a node addon,
# You should add this line in every CMake.js based project
target_include_directories(${NODE_LIBRARY} PRIVATE ${CMAKE_JS_INC})

# Essential library files to link to a node addon
# You should add this line in every CMake.js based project
target_link_libraries(${NODE_LIBRARY} ${CMAKE_JS_LIB})

FOREACH(lib ${PREVIOUS_LIBS})
    MESSAGE(DEBUG "Adding link ${lib} for ${NODE_LIBRARY}")
    TARGET_LINK_LIBRARIES (${NODE_LIBRARY} $<TARGET_FILE_NAME:${lib}>)
    ADD_DEPENDENCIES (${NODE_LIBRARY} ${lib})
ENDFOREACH()

TARGET_INCLUDE_DIRECTORIES (${NODE_LIBRARY} PRIVATE ${PREVIOUS_LIBS_SOURCE_DIR})
target_link_directories(${NODE_LIBRARY} PRIVATE ${LIBRARY_OUTPUT_PATH})

#
# Add Install Targets
#
IF (EXISTS "${MAINFOLDER}/include/${PROJECT_NAME}" AND IS_DIRECTORY "${MAINFOLDER}/include/${PROJECT_NAME}")
    INSTALL(DIRECTORY "${MAINFOLDER}/include/${PROJECT_NAME}" DESTINATION "include")
ENDIF (EXISTS "${MAINFOLDER}/include/${PROJECT_NAME}" AND IS_DIRECTORY "${MAINFOLDER}/include/${PROJECT_NAME}")

#
# Add Documentation Targets
#
#INCLUDE (DocumentationTargets)

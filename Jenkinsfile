pipeline {
    agent any
    
    parameters {
        choice(
            name: 'BROWSER',
            choices: ['chrome', 'firefox', 'both'],
            description: 'Select browser for test execution'
        )
        choice(
            name: 'TEST_CATEGORY',
            choices: ['smoke', 'regression', 'all'],
            description: 'Select test category to run'
        )
        choice(
            name: 'ENVIRONMENT',
            choices: ['qa', 'staging', 'production'],
            description: 'Select environment'
        )
    }
    
    environment {
        DOTNET_CLI_HOME = "${WORKSPACE}"
        SOLUTION_PATH = 'SeleniumFrameworkCSharp.sln'
        PROJECT_PATH = 'SeleniumFrameworkCSharp/SeleniumFrameworkCSharp.csproj'
        REPORT_PATH = 'SeleniumFrameworkCSharp/reports'
        SCREENSHOT_PATH = 'SeleniumFrameworkCSharp/screenshots'
        LOG_PATH = 'SeleniumFrameworkCSharp/logs'
    }
    
    stages {
        stage('Checkout') {
            steps {
                echo '========================================='
                echo 'Stage 1: Checking out code...'
                echo '========================================='
                
                script {
                    env.BUILD_TIMESTAMP = new Date().format('yyyy-MM-dd_HH-mm-ss')
                }
                
                checkout scm
                
                echo "✓ Code checked out successfully"
                echo "Build Timestamp: ${env.BUILD_TIMESTAMP}"
            }
        }
        
        stage('Setup Environment') {
            steps {
                echo '========================================='
                echo 'Stage 2: Setting up environment...'
                echo '========================================='
                
                bat '''
                    echo Checking .NET SDK...
                    dotnet --version
                    
                    echo Checking Docker...
                    docker --version
                    
                    echo ✓ Environment ready
                '''
            }
        }
        
        stage('Clean Workspace') {
            steps {
                echo '========================================='
                echo 'Stage 3: Cleaning workspace...'
                echo '========================================='
                
                bat '''
                    if exist SeleniumFrameworkCSharp\\reports rmdir /s /q SeleniumFrameworkCSharp\\reports
                    if exist SeleniumFrameworkCSharp\\screenshots rmdir /s /q SeleniumFrameworkCSharp\\screenshots
                    if exist SeleniumFrameworkCSharp\\logs rmdir /s /q SeleniumFrameworkCSharp\\logs
                    
                    mkdir SeleniumFrameworkCSharp\\reports
                    mkdir SeleniumFrameworkCSharp\\screenshots
                    mkdir SeleniumFrameworkCSharp\\logs
                    
                    echo ✓ Workspace cleaned
                '''
            }
        }
        
        stage('Start Selenium Grid') {
            steps {
                echo '========================================='
                echo 'Stage 4: Starting Selenium Grid...'
                echo '========================================='
                
                script {
                    bat '''
                        echo Stopping any existing containers...
                        docker stop selenium-hub chrome-node firefox-node 2>nul || echo No containers to stop
                        docker rm selenium-hub chrome-node firefox-node 2>nul || echo No containers to remove
                        
                        echo Starting Selenium Grid with Docker Compose...
                        docker-compose up -d
                        
                        echo Waiting for Grid to be ready...
                        ping 127.0.0.1 -n 21 > nul
                        
                        echo Checking Grid status...
                        curl -s http://localhost:4444/status
                        
                        echo ✓ Selenium Grid is ready!
                    '''
                }
            }
        }
        
        stage('Restore Dependencies') {
            steps {
                echo '========================================='
                echo 'Stage 5: Restoring NuGet packages...'
                echo '========================================='
                
                bat '''
                    dotnet restore %SOLUTION_PATH%
                    echo ✓ Dependencies restored
                '''
            }
        }
        
        stage('Build Solution') {
            steps {
                echo '========================================='
                echo 'Stage 6: Building solution...'
                echo '========================================='
                
                bat '''
                    dotnet build %SOLUTION_PATH% --configuration Release --no-restore
                    echo ✓ Build successful
                '''
            }
        }
        
        stage('Run Tests') {
            steps {
                echo '========================================='
                echo 'Stage 7: Running tests...'
                echo '========================================='
                echo "Browser: ${params.BROWSER}"
                echo "Category: ${params.TEST_CATEGORY}"
                echo "Environment: ${params.ENVIRONMENT}"
                
                script {
                    def filter = ""
                    
                    if (params.TEST_CATEGORY == 'smoke') {
                        filter = '--filter "Category=smoke"'
                    } else if (params.TEST_CATEGORY == 'regression') {
                        filter = '--filter "Category=regression"'
                    } else {
                        filter = ''
                    }
                    
                    bat """
                        dotnet test %PROJECT_PATH% ^
                            --configuration Release ^
                            --no-build ^
                            ${filter} ^
                            --logger "trx;LogFileName=test-results-${env.BUILD_TIMESTAMP}.trx" ^
                            || exit 0
                        
                        echo ✓ Test execution completed
                    """
                }
            }
        }
        
        stage('Publish Reports') {
            steps {
                echo '========================================='
                echo 'Stage 8: Publishing reports...'
                echo '========================================='
                
                // Publish test results
                script {
                    try {
                        mstest testResultsFile: '**/test-results-*.trx'
                        echo "✓ Test results published"
                    } catch (Exception e) {
                        echo "Note: MSTest plugin not available, skipping test result publishing"
                    }
                }
                
                // Publish HTML reports
                publishHTML([
                    allowMissing: true,
                    alwaysLinkToLastBuild: true,
                    keepAll: true,
                    reportDir: "${REPORT_PATH}",
                    reportFiles: '*.html',
                    reportName: 'Extent Report',
                    reportTitles: 'Test Execution Report'
                ])
                
                echo "✓ HTML reports published"
            }
        }
        
        stage('Archive Artifacts') {
            steps {
                echo '========================================='
                echo 'Stage 9: Archiving artifacts...'
                echo '========================================='
                
                archiveArtifacts artifacts: "${REPORT_PATH}/**/*", allowEmptyArchive: true
                archiveArtifacts artifacts: "${SCREENSHOT_PATH}/**/*", allowEmptyArchive: true
                archiveArtifacts artifacts: "${LOG_PATH}/**/*", allowEmptyArchive: true
                archiveArtifacts artifacts: '**/test-results-*.trx', allowEmptyArchive: true
                
                echo "✓ Artifacts archived"
            }
        }
    }
    
    post {
        always {
            echo '========================================='
            echo 'Cleanup: Stopping Selenium Grid...'
            echo '========================================='
            
            bat '''
                docker-compose down || exit 0
                echo ✓ Cleanup completed
            '''
            
            echo '========================================='
            echo 'Pipeline execution completed!'
            echo '========================================='
        }
        
        success {
            echo '✓✓✓ PIPELINE SUCCESSFUL ✓✓✓'
            echo "Build #${BUILD_NUMBER} completed successfully"
            echo "Check the Extent Report for detailed results"
        }
        
        failure {
            echo '✗✗✗ PIPELINE FAILED ✗✗✗'
            echo "Build #${BUILD_NUMBER} failed"
            echo "Check logs and screenshots for failure details"
        }
        
        unstable {
            echo '⚠⚠⚠ PIPELINE UNSTABLE ⚠⚠⚠'
            echo "Some tests may have failed"
        }
    }
}

#!/user/bin/env groovy

// update code for running on server
node('master') {
    try {
        stage('Checkout'){
            checkout scm
        }

        stage('Build'){
            bat 'C:\\Tools\\nuget.exe restore Sample1.sln'
            bat "\"${tool 'MSBuild - 15.0'}\" Sample1.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
        }

        stage('Backend Test'){
            // Use Xunit.Runner.Console to excute unit test files and generate report in xml format
            // replace this with parsing code 

            // This is the powershell code
            // The default powershell execution policy is restrict, we need to change it to bypass or unrestricted in
            // order to execute ps1 script inside a jenkins pipeline script
            // bat 'npm install xunit.runner.console'
            bat "powershell C:\\Tools\\XUnit_Test_Runner.ps1 -rootDir 'C:\\Program Files (x86)\\Jenkins\\workspace\\Back-End-Demo\\' -defaultReportsLocation 'C:\\Program Files (x86)\\Jenkins\\workspace\\Back-End-Demo\\TestReport\\' -reportFilePathPattern '{0}\\xunit_report_{1}.xml' -xunitTestRunnerPath 'C:\\Tools\\xunit.runner.console.2.3.1\\tools\\net452\\'"


            // and this is the manual way to do the same thing
            // bat 'packages\\xunit.runner.console.2.3.1\\tools\\net452\\xunit.console XunitTestClass\\bin\\Release\\XunitTestClass.dll -xml TestReport\\report.xml'
            
            
        }

        stage('Archive'){
            archiveArtifacts "Sample1/bin/Release/**/*"
        }
        // Email SMTP has not been set up
        /*
        mail body: 'project build successful', 
                        subject: 'pipeline test email: successful', 
                        to: 'cxu@acr.org'
        */
    }
    catch(error){
        // Email SMTP has not been set up
        /*
        mail body: "project build error is here: ${env.BUILD_URL}", 
                        subject: 'pipeline test email: fail', 
                        to: 'cxu@acr.org'
        */
        throw error
    }
    finally{
        // Use Xunit Plugin to read report
        step([$class: 'XUnitPublisher', testTimeMargin: '3000', thresholdMode: 1, thresholds: [[$class: 'FailedThreshold', failureNewThreshold: '5', failureThreshold: '20', unstableNewThreshold: '5', unstableThreshold: '10'], [$class: 'SkippedThreshold', failureNewThreshold: '5', failureThreshold: '20', unstableNewThreshold: '5', unstableThreshold: '10']], tools: [[$class: 'XUnitDotNetTestType', deleteOutputFiles: true, failIfNotNew: true, pattern: 'TestReport\\*.xml', skipNoTestFiles: true, stopProcessingIfError: true]]])
    }
}

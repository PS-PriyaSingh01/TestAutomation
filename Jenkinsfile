pipeline {
    agent any
  
    environment {
		TK_NAMESPACE = 'testkube'
		TK_VERSION = '1.16.7'
    }
	
    stages {		
		stage('Setup Testkube') {
            steps {
                script {
                    // setupTestkube()
                    bat 'kubectl testkube run test priya'
                }
            }
        }
    }
}
pipeline {
    agent any
  
    environment {
		TK_NAMESPACE = 'testkube'
    }
	
    stages {		
		stage('Example') {
            steps {
                script {
                    //setupTestkube()
                    sh 'testkube run test priya'
                }
            }
        }
    }
}
pipeline {
    agent any
  
    environment {
		TK_NAMESPACE = 'testkube'
		TK_VERSION = '1.16.7'
		KUBECONFIG = 'C:\\Users\\Priya.Singh\\.kube\\config'
    }
	
    stages {		
		stage('Setup Testkube') {
            steps {
                script {
                    // setupTestkube()
                    // bat 'kubectl testkube run test priya'
					withEnv(["PATH+KUBECTL=${tool 'kubectl'}"]) {
                    bat '''
                        echo %KUBECONFIG%
                        kubectl config view
                        testkube run test priya
                    '''
                }
            }
        }
    }
}
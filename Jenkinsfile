pipeline {
    agent any
    tools {
        dockerTool 'docker'
    }
    stages {
        stage('Cloning git') {
            steps {
                git url: 'https://github.com/donganh181/kiosk-solution', branch: 'production', credentialsId: 'github'
            }
        }
        stage('Build Image') {
            steps {
                sh "docker build -t longpc/kiosk-solution . --no-cache"
            }
        }
        stage('Test') {
            steps {
                echo 'Testing..'
            }
        }
        stage("Login dockerhub") {
            steps {
               withCredentials([usernamePassword(credentialsId: 'docker-hub', passwordVariable: 'pass', usernameVariable: 'user')]) {
                    sh "docker login -u $user -p $pass"
               }
            }
        }
        stage("Push Image") {
            steps {
               sh "docker push longpc/kiosk-solution"
            }
        }
    }
}
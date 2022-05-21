pipeline {
    agent any
    tools {
        maven 'maven 3.8.4'
        dockerTool 'docker'
    }
    stages {
        stage('Cloning git') {
            steps {
                git url: 'https://github.com/donganh181/kiosk-solution', credentialsId: 'github'
            }
        }
        stage('Build Image') {
            steps {
                sh "docker build kiosk-solution --no-cache -t longpc/kiosk-solution ."
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